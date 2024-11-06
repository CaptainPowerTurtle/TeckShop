using Catalog.Application;
using Catalog.Application.EventHandlers.DomainEvents;
using Catalog.Application.EventHandlers.IntegrationEvents;
using Catalog.Infrastructure.Persistence;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Scrutor;
using TeckShop.Core.Database;
using TeckShop.Core.Events;
using TeckShop.Infrastructure;
using TeckShop.Infrastructure.Auth;
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
            var applicationAssembly = typeof(ICatalogApplication).Assembly;
            var dbContextAssembly = typeof(AppDbContext).Assembly;

            var swaggerDocumentOptions = new List<Action<DocumentOptions>>();

            builder.Services.AddKeycloak(builder.Configuration, builder.Environment);
            builder.AddInfrastructure(swaggerDocumentOptions, applicationAssembly: applicationAssembly, enableSwagger: enableSwagger, enableFastEndpoints: enableFastEndpoints);
            builder.Services.AddEfDbContext<AppDbContext>(builder.Configuration, builder.Environment, dbContextAssembly);
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
                    cfg.Host(builder.Configuration["RabbitMqOptions:Host"]);
                    cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("catalog", false));
                });
            });

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
            app.UseInfrastructure(app.Environment, enableSwagger, enableFastEndpoints);
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
        }
    }
}
