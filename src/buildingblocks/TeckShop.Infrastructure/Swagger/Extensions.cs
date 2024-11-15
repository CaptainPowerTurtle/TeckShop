using FastEndpoints.Swagger;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NSwag;
using NSwag.Generation.Processors.Security;
using TeckShop.Infrastructure.Options;

namespace TeckShop.Infrastructure.Swagger
{
    internal static class Extensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwaggerGen(
                config: config =>
                {
                    config.Path = "/docs/{documentName}/openapi.json";
                    config.PostProcess = (swagger, httpReq) =>
                    {
                        OpenApiServer openApiServer = new()
                        {
                            Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{httpReq.PathBase.Value}"
                        };

                        swagger.Servers.Add(openApiServer);
                    };
                },
                uiConfig: config =>
                {
                    config.DocumentPath = "/docs/{documentName}/openapi.json";
                    config.Path = "/docs";
                    config.DefaultModelsExpandDepth = -1;
                    config.DocExpansion = "list";
                });
        }

        internal static void AddSwaggerExtension(
            this IServiceCollection services,
            IConfiguration configuration,
            IList<Action<DocumentOptions>> options,
            KeycloakAuthenticationOptions? keycloakAuthenticationOptions = null)
        {
            SwaggerOptions swaggerOptions = services.BindValidateReturn<SwaggerOptions>(configuration);

            Action<DocumentOptions> document = new(options =>
            {
                options.EnableJWTBearerAuth = false;
                options.MaxEndpointVersion = 0;
                options.DocumentSettings = setting =>
                {
                    setting.Version = "v0";
                    setting.Title = swaggerOptions.Title;
                    setting.DocumentName = "Initial Release";
                    setting.Description = swaggerOptions.Description;

                    if (keycloakAuthenticationOptions != null)
                    {
                        setting.AddSecurity("oAuth2", SwaggerAuth.AddOAuthScheme(keycloakAuthenticationOptions.KeycloakTokenEndpoint, keycloakAuthenticationOptions.KeycloakUrlRealm + "protocol/openid-connect/auth", keycloakAuthenticationOptions.KeycloakTokenEndpoint));
                    }

                    setting.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("oAuth2"));
                };
            });

            options.Add(document);

            foreach (Action<DocumentOptions> option in options)
            {
                services.SwaggerDocument(option);
            }
        }
    }
}
