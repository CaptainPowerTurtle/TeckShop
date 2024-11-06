using FastEndpoints.Swagger;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NSwag;
using NSwag.Generation.Processors.Security;
using TeckShop.Infrastructure.Options;

namespace TeckShop.Infrastructure.Swagger
{
    internal static class Extensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerGen(
                config: config =>
                {
                    config.Path = "/docs/{documentName}/openapi.json";
                    config.PostProcess = (swagger, httpReq) =>
                    {
                        OpenApiServer openApiServer = new OpenApiServer()
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

        internal static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration, IList<Action<DocumentOptions>> options)
        {
            var swaggerOptions = services.BindValidateReturn<SwaggerOptions>(configuration);
            var keycloakOptions = configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>()!;

            Action<DocumentOptions> document = new Action<DocumentOptions>(options =>
            {
                options.EnableJWTBearerAuth = false;
                options.MaxEndpointVersion = 0;
                options.DocumentSettings = setting =>
                {
                    setting.Version = "v0";
                    setting.Title = swaggerOptions.Title;
                    setting.DocumentName = "Initial Release";
                    setting.Description = swaggerOptions.Description;
                    setting.AddSecurity("oAuth2", SwaggerAuth.AddOAuthScheme(keycloakOptions.KeycloakTokenEndpoint, keycloakOptions.KeycloakUrlRealm + "protocol/openid-connect/auth", keycloakOptions.KeycloakTokenEndpoint));
                    setting.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("oAuth2"));
                };
            });

            options.Add(document);

            foreach (var option in options)
            {
                services.SwaggerDocument(option);
            }
        }
    }
}
