using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TeckShop.Core.Database;
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
        /// Add db context.
        /// </summary>
        /// <typeparam name="TContext"/>
        /// <param name="builder">The builder.</param>
        /// <param name="assembly"></param>
        /// <param name="connectionString"></param>
        public static void AddCustomDbContext<TContext>(this WebApplicationBuilder builder, Assembly assembly, string connectionString)
            where TContext : BaseDbContext
        {
            builder.Services.AddSingleton<SoftDeleteInterceptor>();
            builder.Services.AddSingleton<AuditingInterceptor>();
            builder.Services.AddSingleton<DomainEventInterceptor>();

            builder.Services.AddDbContext<TContext>((sp, options) =>
            {
                options.UseNpgsql(connectionString, migration => migration.MigrationsAssembly(assembly.FullName));
                options.AddInterceptors(
                    sp.GetRequiredService<SoftDeleteInterceptor>(),
                    sp.GetRequiredService<AuditingInterceptor>(),
                    sp.GetRequiredService<DomainEventInterceptor>());
            });

            builder.EnrichNpgsqlDbContext<TContext>(config =>
            {
                config.ConnectionString = connectionString;
                config.DisableTracing = true;
            });

            builder.Services.AddHealthChecks().AddNpgSql(
                connectionString: connectionString,
                tags: ["db", "sql", "postgres"]);

            builder.Services.AddScoped<TContext>();
            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        }
    }
}
