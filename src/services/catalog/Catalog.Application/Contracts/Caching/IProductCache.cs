using Catalog.Domain.Entities.Products;
using TeckShop.Core.Caching;

namespace Catalog.Application.Contracts.Caching
{
    /// <summary>
    /// Product cache interface.
    /// </summary>
    public interface IProductCache : IGenericCacheService<Product, Guid>
    {
    }
}
