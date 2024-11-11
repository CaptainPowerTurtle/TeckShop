using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Entities.Products;
using TeckShop.Persistence.Caching;
using ZiggyCreatures.Caching.Fusion;

namespace Catalog.Infrastructure.Caching
{
    /// <summary>
    /// The product cache.
    /// </summary>
    public class ProductCache : GenericCacheService<Product, Guid>, IProductCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCache"/> class.
        /// </summary>
        /// <param name="brandCache">The brand cache.</param>
        /// <param name="brandRepository">The brand repository.</param>
        public ProductCache(IFusionCache brandCache, IProductRepository brandRepository) : base(brandCache, brandRepository)
        {
        }
    }
}
