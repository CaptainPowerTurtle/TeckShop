using System.Diagnostics;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalog.MigrationService
{
    internal class ApiDbInitializer(
        IServiceProvider serviceProvider,
        IHostEnvironment hostEnvironment,
        IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {
        private readonly ActivitySource _activitySource = new(hostEnvironment.ApplicationName);

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using Activity? activity = _activitySource.StartActivity(hostEnvironment.ApplicationName, ActivityKind.Client);

            try
            {
                using IServiceScope scope = serviceProvider.CreateScope();
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await EnsureDatabaseAsync(dbContext, stoppingToken);
                await RunMigrationAsync(dbContext, stoppingToken);
            }
            catch (Exception exception)
            {
                activity?.AddException(exception);
                throw;
            }

            hostApplicationLifetime.StopApplication();
        }

        private static async Task EnsureDatabaseAsync(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            IRelationalDatabaseCreator dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

            IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Create the database if it does not exist.
                // Do this first so there is then a database to start a transaction against.
                if (!await dbCreator.ExistsAsync(cancellationToken))
                {
                    await dbCreator.CreateAsync(cancellationToken);
                }
            });
        }

        private static async Task RunMigrationAsync(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Run migration in a transaction to avoid partial migration if it fails.
                await dbContext.Database.MigrateAsync(cancellationToken);
            });
        }
    }
}
