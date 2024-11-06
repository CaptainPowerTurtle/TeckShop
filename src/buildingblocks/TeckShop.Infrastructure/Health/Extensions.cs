using HealthChecks.ApplicationStatus.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TeckShop.Core.Exceptions;
using TeckShop.Infrastructure.Caching;
using TeckShop.Infrastructure.Options;

namespace TeckShop.Infrastructure.Health
{
    internal static class Extensions
    {
        internal static void AddHealthcheckService(this IServiceCollection services, IConfiguration configuration)
        {
            var healthOptions = services.BindValidateReturn<HealthOptions>(configuration);
            if (healthOptions.Postgres)
            {
                services.AddHealthChecks().AddNpgSql(
                    connectionString: configuration["DatabaseOptions:ConnectionString"] ?? throw new ConfigurationMissingException("Database"),
                    tags: new string[] { "db", "sql", "postgres" });
            }

            if (healthOptions.Redis)
            {
                var cacheOptions = services.BindValidateReturn<CachingOptions>(configuration);

                services.AddHealthChecks().AddRedis($"{cacheOptions.RedisURL},password={cacheOptions.Password}", tags: new string[] { "cache", "redis" });
            }

            if (healthOptions.RabbitMq)
            {
                services.AddHealthChecks().AddRabbitMQ(rabbitConnectionString: configuration["RabbitMqOptions:Host"] ?? throw new ConfigurationMissingException("RabbitMQ"), tags: new string[] { "messagebus", "rabbitmq" });
            }

            if (healthOptions.OpenIdConnectServer)
            {
                services.AddHealthChecks().AddIdentityServer(new Uri(uriString: $"{configuration["Keycloak:auth-server-url"]}/realms/{configuration["Keycloak:realm"]}/"), tags: new string[] { "openId", "identity", "keycloak" }, failureStatus: HealthStatus.Degraded);
            }

            if (healthOptions.ApplicationStatus) services.AddHealthChecks().AddApplicationStatus();
        }

        internal static void UseHealthcheckService(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecksPrometheusExporter("/api/healthmetrics");
        }
    }
}
