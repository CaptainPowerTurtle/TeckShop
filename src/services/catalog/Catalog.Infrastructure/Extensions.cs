using System.Reflection;
using Catalog.Application;
using Catalog.Application.EventHandlers.DomainEvents;
using Catalog.Application.EventHandlers.IntegrationEvents;
using Catalog.Infrastructure.Persistence;
using FastEndpoints.Swagger;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Scrutor;
using TeckShop.Core.Database;
using TeckShop.Core.Events;
using TeckShop.Core.Exceptions;
using TeckShop.Infrastructure;
using TeckShop.Infrastructure.Auth;
using TeckShop.Infrastructure.Multitenant;
using TeckShop.Persistence.Database;
using TeckShop.Persistence.Database.EFCore;
namespace Catalog.Infrastructure
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add catalog infrastructure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="enableSwagger">If true, enable swagger.</param>
        /// <param name="enableFastEndpoints">If true, enable fast endpoints.</param>
        public static void AddCatalogInfrastructure(this WebApplicationBuilder builder, bool enableSwagger = true, bool enableFastEndpoints = true)
        {
            Assembly applicationAssembly = typeof(ICatalogApplication).Assembly;
            Assembly dbContextAssembly = typeof(AppDbContext).Assembly;

            KeycloakAuthenticationOptions keycloakOptions = builder.Configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>() ?? throw new ConfigurationMissingException("Keycloak");

            string postgresConnectionString = builder.Configuration.GetConnectionString("catalogdb") ?? throw new ConfigurationMissingException("Database");
            string rabbitmqConnectionString = builder.Configuration.GetConnectionString("rabbitmq") ?? throw new ConfigurationMissingException("RabbitMq");

            List<Action<DocumentOptions>> swaggerDocumentOptions = new();

            builder.Services.AddKeycloak(builder.Configuration, builder.Environment, keycloakOptions);

            builder.AddInfrastructure(swaggerDocumentOptions, keycloakOptions, applicationAssembly: applicationAssembly, enableSwagger: enableSwagger, enableFastEndpoints: enableFastEndpoints);

            builder.AddCustomDbContext<AppDbContext>(dbContextAssembly, postgresConnectionString);

            string? rabbitmqConnection = builder.Configuration.GetConnectionString("rabbitmq");
            builder.Services.AddMediator(consumer =>
            {
                consumer.AddConsumer<BrandCreatedDomainEventConsumer>();
            });
            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<BrandCreatedIntegrationEventConsumer>();
                config.AddEntityFrameworkOutbox<AppDbContext>(option =>
                {
                    option.UsePostgres();
                    option.UseBusOutbox();
                });
                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Publish<DomainEvent>(message => message.Exclude = true);
                    cfg.DeployPublishTopology = true;
                    cfg.UseDelayedRedelivery(message => message.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
                    cfg.UseMessageRetry(message => message.Immediate(5));
                    cfg.Host(rabbitmqConnection);
                    cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("catalog", false));
                });
            });
            builder.Services.AddHealthChecks().AddRabbitMQ(rabbitConnectionString: rabbitmqConnectionString, tags: ["messagebus", "rabbitmq"]);

            builder.Services.AddMultitenantExtension(keycloakOptions);

            // Automatically register services.
            builder.Services.Scan(selector => selector
            .FromAssemblies(
                applicationAssembly,
                dbContextAssembly)
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime());

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();

            ////builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        /// <summary>
        /// Use catalog infrastructure.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="enableSwagger">If true, enable swagger.</param>
        /// <param name="enableFastEndpoints">If true, enable fast endpoints.</param>
        public static void UseCatalogInfrastructure(this WebApplication app, bool enableSwagger = true, bool enableFastEndpoints = true)
        {
            app.UseMultitenantExtension();
            app.UseInfrastructure(enableSwagger, enableFastEndpoints);
        }
    }
}
