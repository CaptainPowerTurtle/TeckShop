using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Entities.Categories;
using Microsoft.AspNetCore.Http;
using TeckShop.Persistence.Database.EFCore;

namespace Catalog.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// The category repository.
    /// </summary>
    public class CategoryRepository : GenericRepository<Category, Guid>, ICategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContextAccessor"></param>
        public CategoryRepository(BaseDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
}
