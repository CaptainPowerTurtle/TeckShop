using Catalog.Domain.Entities.Brands;
using TeckShop.Core.Caching;

namespace Catalog.Application.Contracts.Caching
{
    /// <summary>
    /// Branc cache interface.
    /// </summary>
    public interface IBrandCache : IGenericCacheService<Brand, Guid>
    {
    }
}
