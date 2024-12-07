using System.Reflection;
using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using IdempotentAPI.Extensions.DependencyInjection;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using TeckShop.Core.Auth;
using TeckShop.Core.Exceptions;
using TeckShop.Infrastructure.Behaviors;
using TeckShop.Infrastructure.Caching;
using TeckShop.Infrastructure.Logging;
using TeckShop.Infrastructure.Mapping;
using TeckShop.Infrastructure.Options;
using TeckShop.Infrastructure.Swagger;

namespace TeckShop.Infrastructure
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Allow all origins.
        /// </summary>
        public const string AllowAllOrigins = "AllowAll";

        /// <summary>
        /// Add the infrastructure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// /// <param name="swaggerDocumentOptions">The swagger document options.</param>
        /// <param name="keycloakOptions"></param>
        /// <param name="applicationAssembly">The application assembly.</param>
        /// <param name="enableSwagger">If true, enable swagger.</param>
        /// <param name="enableFastEndpoints">If true, enable fast endpoints.</param>
        /// <param name="addCaching">If true, add caching.</param>
        public static void AddInfrastructure(
            this WebApplicationBuilder builder,
            IList<Action<DocumentOptions>> swaggerDocumentOptions,
            KeycloakAuthenticationOptions? keycloakOptions = null,
            Assembly? applicationAssembly = null,
            bool enableSwagger = true,
            bool enableFastEndpoints = true,
            bool addCaching = true)
        {
            AppOptions appOptions = builder.Services.BindValidateReturn<AppOptions>(builder.Configuration);

            builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: AllowAllOrigins, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            builder.Services.AddHttpContextAccessor();
            if (addCaching)
            {
                string redisConnectionString = builder.Configuration.GetConnectionString("redis") ?? throw new ConfigurationMissingException("Redis");
                builder.AddCachingService(redisConnectionString, builder.Environment);
                builder.Services.AddIdempotentMinimalAPI(new IdempotentAPI.Core.IdempotencyOptions() { HeaderKeyName = "Idempotency-Key" });
            }

            if (enableFastEndpoints)
            {
                builder.Services.AddFastEndpoints(ep =>
                {
                if (applicationAssembly is not null)
                {
                    ep.Assemblies = [applicationAssembly];
                }
            }).AddIdempotency();
            }

            builder.ConfigureSerilog(appOptions.Name);
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            if (applicationAssembly != null)
            {
                builder.Services.AddMapsterExtension(applicationAssembly);
                builder.Services.AddValidatorsFromAssembly(applicationAssembly, filter: filter => filter.ValidatorType.BaseType?.GetGenericTypeDefinition() != typeof(FastEndpoints.Validator<>));
                builder.Services.AddMediatR(option =>
                {
                    option.RegisterServicesFromAssembly(applicationAssembly);
                    option.AddOpenBehavior(typeof(LoggingBehavior<,>));
                    option.AddOpenBehavior(typeof(TransactionalBehavior<,>));
                });
            }

            if (enableSwagger)
            {
                builder.Services.AddSwaggerExtension(builder.Configuration, swaggerDocumentOptions, keycloakOptions);
            }

            if (keycloakOptions is not null)
            {
                builder.Services
                    .AddClientCredentialsTokenManagement()
                    .AddClient(
                    AuthConstants.TeckShop,
                    client =>
                    {
                        client.ClientId = keycloakOptions.Resource;
                        client.ClientSecret = keycloakOptions.Credentials.Secret;
                        client.TokenEndpoint = keycloakOptions.KeycloakTokenEndpoint;
                    }
                    );
            }

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        /// <summary>
        /// Use the infrastructure.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="enableSwagger">If true, enable swagger.</param>
        /// <param name="enableFastEndpoints">If true, enable fast endpoints.</param>
        public static void UseInfrastructure(this WebApplication app, bool enableSwagger = true, bool enableFastEndpoints = true)
        {
            // Preserve Order
            app.UseCors(AllowAllOrigins);
            app.UseForwardedHeaders();
            app.UseAuthentication();
            app.UseAuthorization();
            if (enableFastEndpoints)
            {
                app.UseOutputCache().UseFastEndpoints(config =>
            {
                config.Errors.UseProblemDetails(error =>
                {
                    error.TitleTransformer = pd => pd.Status switch
                    {
                        400 => "Validation Error",
                        404 => "Not Found",
                        401 => "Unauthorized",
                        403 => "Forbidden",
                        _ => "One or more errors occurred!"
                    };
                });
                config.Serializer.Options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                config.Endpoints.RoutePrefix = "api";
                config.Versioning.Prefix = "v";
                config.Versioning.PrependToRoute = true;
                config.Versioning.DefaultVersion = 1;
            });
            }

            app.MapHealthChecks("/readiness").AllowAnonymous();
            if (enableSwagger)
            {
                app.UseSwaggerExtension();
            }
        }
    }
}
