using FastEndpoints;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
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
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddKeycloak(this IServiceCollection services, IConfiguration config, IHostEnvironment hostEnvironment)
        {
            bool isProductions = hostEnvironment.IsProduction();
            KeycloakAuthenticationOptions authenticationOptions = new();
            config.GetSection(KeycloakAuthenticationOptions.Section).Bind(authenticationOptions, opt => opt.BindNonPublicProperties = true);

            services.AddKeycloakWebApiAuthentication(config, options =>
            {
                options.IncludeErrorDetails = true;
                options.Authority = authenticationOptions.KeycloakUrlRealm;
                options.Audience = authenticationOptions.Resource;
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
                            var error = context.Error != null ? context.Error : "Unauthorized";
                            var description = context.ErrorDescription != null ? context.ErrorDescription : "You are not authorized to access this resource";
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
            return services;
        }
    }
}
