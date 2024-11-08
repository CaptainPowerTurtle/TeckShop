using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using TeckShop.Core.Domain;

namespace TeckShop.Persistence.Database.EFCore
{
    /// <summary>
    /// The base db context.
    /// </summary>
    public abstract class BaseDbContext : DbContext, IBaseDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        protected BaseDbContext(DbContextOptions options) : base(options)

        {
        }

        /// <summary>
        /// On model creating.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AppendGlobalQueryFilter<ISoftDeletable>(entity => !entity.IsDeleted);
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// On configuring.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();
        }
    }
}
