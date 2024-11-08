using Catalog.Domain.Entities.Brands;
using Catalog.Domain.Entities.Products;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TeckShop.Persistence.Database.EFCore;

namespace Catalog.Infrastructure.Persistence
{
    /// <summary>
    /// The app db context.
    /// </summary>
    public class AppDbContext : BaseDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

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

            modelBuilder.Entity<Brand>()
                .HasIndex(brand => brand.IsDeleted)
                .HasFilter("Brands.IsDeleted = 0");

            modelBuilder.Entity<Product>()
                .HasIndex(product => product.IsDeleted)
                .HasFilter("Products.IsDeleted = 0");
        }

        /// <summary>
        /// Gets or sets the brands.
        /// </summary>
        public DbSet<Brand> Brands { get; set; }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public DbSet<Product> Products { get; set; }
    }
}
