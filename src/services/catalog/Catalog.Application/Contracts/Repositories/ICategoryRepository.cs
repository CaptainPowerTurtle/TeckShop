using Catalog.Domain.Entities.Categories;
using TeckShop.Core.Database;

namespace Catalog.Application.Contracts.Repositories
{
    /// <summary>
    /// Category repository interface.
    /// </summary>
    public interface ICategoryRepository : IGenericRepository<Category, Guid>
    {
    }
}
