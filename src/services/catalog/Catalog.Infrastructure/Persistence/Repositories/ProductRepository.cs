using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using TeckShop.Core.Pagination;
using TeckShop.Persistence.Database.EFCore;

namespace Catalog.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// The product repository.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </remarks>
    /// <param name="context">The context.</param>
    /// <param name="httpContextAccessor">The http context accessor.</param>
    public class ProductRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : GenericRepository<Product, Guid>(context, httpContextAccessor), IProductRepository
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly AppDbContext _context = context;

        /// <summary>
        /// Get paged products asynchronously.
        /// </summary>
        /// <typeparam name="TProductResponse"/>
        /// <param name="page">The page.</param>
        /// <param name="size">The size.</param>
        /// <param name="nameFilter"></param>
        /// <param name="sortDescending"></param>
        /// <param name="sortValue"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<PagedList<ProductResponse>>]]></returns>
        public async Task<PagedList<TProductResponse>> GetPagedProductsAsync<TProductResponse>(int page, int size, string? nameFilter, bool? sortDescending, string? sortValue, CancellationToken cancellationToken = default)
        {
            IQueryable<Product> queryable = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(nameFilter))
            {
                nameFilter = nameFilter.ToLowerInvariant();
                queryable = queryable.Where(brand => brand.Name.Contains(nameFilter));
            }

            if (!string.IsNullOrEmpty(sortValue) && sortDescending is not null)
            {
                sortValue = sortValue.ToLower();
                switch (sortValue)
                {
                    case "name":
                        queryable = sortDescending.Value ? queryable.OrderByDescending(product => product.Name) : (IQueryable<Product>)queryable.OrderBy(product => product.Name);
                        break;
                    case "productsku":
                        queryable = sortDescending.Value ? queryable.OrderByDescending(product => product.ProductSKU) : (IQueryable<Product>)queryable.OrderBy(product => product.ProductSKU);
                        break;
                    case "isactive":
                        queryable = sortDescending.Value ? queryable.OrderByDescending(product => product.IsActive) : (IQueryable<Product>)queryable.OrderBy(product => product.IsActive);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                queryable = queryable.OrderByDescending(product => product.CreatedOn);
            }

            return await queryable.ApplyPagingAsync<Product, TProductResponse>(page, size, cancellationToken);
        }
    }
}
