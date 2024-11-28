using Catalog.Domain.Entities.Brands;
using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Entities.ProductPrices;
using Catalog.Domain.Entities.ProductPriceTypes;
using Catalog.Domain.Entities.Products;
using Catalog.Domain.Entities.Promotions;
using Catalog.Domain.Entities.Suppliers;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TeckShop.Infrastructure.Multitenant;
using TeckShop.Persistence.Database.EFCore;

namespace Catalog.Infrastructure.Persistence
{
    /// <summary>
    /// The app db context.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </remarks>
    /// <param name="multiTenantContextAccessor"></param>
    /// <param name="options">The options.</param>
    public class AppDbContext(IMultiTenantContextAccessor<TeckShopTenant> multiTenantContextAccessor, DbContextOptions<AppDbContext> options) : BaseDbContext(options, multiTenantContextAccessor)
    {
        /// <summary>
        /// On model creating.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            modelBuilder.Entity<Brand>().IsMultiTenant();
        }

        /// <summary>
        /// Gets or sets the brands.
        /// </summary>
        public required DbSet<Brand> Brands { get; set; }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public required DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        public required DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the product prices.
        /// </summary>
        public required DbSet<ProductPrice> ProductPrices { get; set; }

        /// <summary>
        /// Gets or sets the product price types.
        /// </summary>
        public required DbSet<ProductPriceType> ProductPriceTypes { get; set; }

        /// <summary>
        /// Gets or sets the promotions.
        /// </summary>
        public required DbSet<Promotion> Promotions { get; set; }

        /// <summary>
        /// Gets or sets the suppliers.
        /// </summary>
        public required DbSet<Supplier> Suppliers { get; set; }
    }
}
