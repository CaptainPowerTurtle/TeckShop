using Catalog.Domain.Entities.ProductPriceTypes;
using TeckShop.Core.Database;

namespace Catalog.Application.Contracts.Repositories
{
    /// <summary>
    /// ProductPriceType repository interface.
    /// </summary>
    public interface IProductPriceTypeRepository : IGenericRepository<ProductPriceType, Guid>
    {
    }
}
