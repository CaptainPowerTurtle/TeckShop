using Catalog.Domain.Entities.Products;
using TeckShop.Core.Caching;

namespace Catalog.Application.Contracts.Caching
{
    /// <summary>
    /// Product cache interface.
    /// </summary>
    public interface IProductCache : IGenericCacheService<Product, Guid>
    {
        /// <summary>
        /// Get or set by sku.
        /// </summary>
        /// <param name="productSku">The product sku.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<Product?>]]></returns>
        public Task<Product?> GetOrSetBySku(string productSku, CancellationToken cancellationToken = default);
    }
}
