using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Entities.ProductPriceTypes;
using Microsoft.AspNetCore.Http;
using TeckShop.Persistence.Database.EFCore;

namespace Catalog.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// The product price type repository.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="httpContextAccessor">The http context accessor.</param>
    public class ProductPriceTypeRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : GenericRepository<ProductPriceType, Guid>(context, httpContextAccessor), IProductPriceTypeRepository
    {
    }
}
