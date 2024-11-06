using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TeckShop.Core.Database;
using TeckShop.Infrastructure.Options;
using TeckShop.Persistence.Database.EFCore;
using TeckShop.Persistence.Database.EFCore.Interceptors;

namespace TeckShop.Persistence.Database
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add ef db context.
        /// </summary>
        /// <typeparam name="TContext"/>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="webHostEnvironment">The web host environment.</param>
        /// <param name="dbContextAssembly">The db context assembly.</param>
        /// <exception cref="ArgumentNullException">.</exception>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddEfDbContext<TContext>(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, Assembly dbContextAssembly)
            where TContext : BaseDbContext
        {
            var databaseOptions = services.BindValidateReturn<DatabaseOptions>(configuration);
            if (string.IsNullOrWhiteSpace(databaseOptions.DatabaseName))
            {
                var databaseName = nameof(databaseOptions.DatabaseName);
                throw new ArgumentNullException(databaseName);
            }

            if (string.IsNullOrWhiteSpace(databaseOptions.ConnectionString))
            {
                var connectionString = nameof(databaseOptions.ConnectionString);
                throw new ArgumentNullException(connectionString);
            }

            services.AddSingleton<SoftDeleteInterceptor>();
            services.AddSingleton<AuditingInterceptor>();
            services.AddSingleton<DomainEventInterceptor>();

            services.AddDbContext<TContext>((sp, options) => options
            .UseNpgsql(databaseOptions.ConnectionString, migration => migration.MigrationsAssembly(dbContextAssembly.FullName))
            .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>(), sp.GetRequiredService<AuditingInterceptor>(), sp.GetRequiredService<DomainEventInterceptor>())
            );
            services.AddScoped(typeof(TContext));
            services.AddScoped<IBaseDbContext>(sp => sp.GetRequiredService<TContext>());
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            return services;
        }
    }
}
