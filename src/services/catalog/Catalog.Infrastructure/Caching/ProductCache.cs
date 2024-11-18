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
    /// <param name="cache">The cache.</param>
    /// <param name="productRepository">The product repository.</param>
    public class ProductCache(IFusionCache cache, IProductRepository productRepository) : GenericCacheService<Product, Guid>(cache, productRepository), IProductCache
    {
        /// <summary>
        /// The product repository.
        /// </summary>
        private readonly IProductRepository _productRepository = productRepository;

        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IFusionCache _cache = cache;

        /// <summary>
        /// Get or set by id asynchronously.
        /// </summary>
        /// <param name="productSku">The product sku.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<Product?>]]></returns>
        public async Task<Product?> GetOrSetBySku(string productSku, CancellationToken cancellationToken = default)
        {
            string key = GenerateCacheKey(productSku);

            return await _cache.GetOrSetAsync<Product?>(
                key,
                async (context, ct) =>
                {
                    Product? result = await _productRepository.FindOneAsync(product => product.ProductSKU.ToLower().Equals(productSku), enableTracking: false, cancellationToken: cancellationToken);
                    if (result is null)
                    {
                        context.Options.Duration = TimeSpan.FromMinutes(5);
                    }

                    return result;
                },
                token: cancellationToken);
        }
    }
}
