using EntityFramework.Exceptions.PostgreSQL;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeckShop.Core.Domain;
using TeckShop.Infrastructure.Multitenant;

namespace TeckShop.Persistence.Database.EFCore
{
    /// <summary>
    /// The base db context.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BaseDbContext"/> class.
    /// </remarks>
    /// <param name="options">The options.</param>
    /// <param name="multiTenantContextAccessor"></param>
    public class BaseDbContext(DbContextOptions options, IMultiTenantContextAccessor<TeckShopTenant> multiTenantContextAccessor) : MultiTenantDbContext(multiTenantContextAccessor, options)
    {
        /// <summary>
        /// On model creating.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AppendGlobalQueryFilter<ISoftDeletable>(entity => !entity.IsDeleted);
            modelBuilder.ConfigureMultiTenant();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// On configuring.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();

            if (!string.IsNullOrWhiteSpace(multiTenantContextAccessor?.MultiTenantContext.TenantInfo?.ConnectionString))
            {
                optionsBuilder.UseNpgsql(multiTenantContextAccessor.MultiTenantContext.TenantInfo.ConnectionString!);
            }
        }
    }
}
