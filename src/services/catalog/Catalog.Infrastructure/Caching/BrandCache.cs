using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Entities.Brands;
using TeckShop.Persistence.Caching;
using ZiggyCreatures.Caching.Fusion;

namespace Catalog.Infrastructure.Caching
{
    /// <summary>
    /// The brand cache.
    /// </summary>
    public class BrandCache : GenericCacheService<Brand, Guid>, IBrandCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrandCache"/> class.
        /// </summary>
        /// <param name="brandCache">The brand cache.</param>
        /// <param name="brandRepository">The brand repository.</param>
        public BrandCache(IFusionCache brandCache, IBrandRepository brandRepository) : base(brandCache, brandRepository)
        {
        }
    }
}
