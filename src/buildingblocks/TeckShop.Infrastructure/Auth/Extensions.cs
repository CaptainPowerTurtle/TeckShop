using FastEndpoints;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace TeckShop.Infrastructure.Auth
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add the keycloak.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        /// <param name="hostEnvironment">The host environment.</param>
        /// <param name="keycloakOptions"></param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddKeycloak(this IServiceCollection services, IConfiguration config, IHostEnvironment hostEnvironment, KeycloakAuthenticationOptions keycloakOptions)
        {
            bool isProductions = hostEnvironment.IsProduction();
            KeycloakAuthenticationOptions authenticationOptions = new();
            config.GetSection(KeycloakAuthenticationOptions.Section).Bind(authenticationOptions, opt => opt.BindNonPublicProperties = true);

            services.AddKeycloakWebApiAuthentication(config, options =>
            {
                options.IncludeErrorDetails = true;
                options.Authority = keycloakOptions.KeycloakUrlRealm;
                options.Audience = keycloakOptions.Resource;
                options.RequireHttpsMetadata = isProductions;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    RequireAudience = isProductions,
                    ValidateAudience = isProductions,
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            string error = context.Error != null ? context.Error : "Unauthorized";
                            string description = context.ErrorDescription != null ? context.ErrorDescription : "You are not authorized to access this resource";
                            await context.Response.SendErrorsAsync([new(error, description)], 401);
                        }

                        return;
                    },
                    OnForbidden = async context =>
                    {
                        await context.Response.SendErrorsAsync([new("Forbidden", "You don't have permission to access this resource")], 403);
                    },
                };
            });
            services.AddAuthorization().AddKeycloakAuthorization().AddAuthorizationServer(config);
            services.AddHealthChecks().AddIdentityServer(new Uri(uriString: $"{keycloakOptions.AuthServerUrl}/realms/{keycloakOptions.Realm}/"), tags: ["openId", "identity", "keycloak"], failureStatus: HealthStatus.Degraded);
            return services;
        }
    }
}
