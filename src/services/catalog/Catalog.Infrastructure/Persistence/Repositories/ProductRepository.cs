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
        /// <param name="keyword">The keyword.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<PagedList<ProductResponse>>]]></returns>
        public async Task<PagedList<TProductResponse>> GetPagedProductsAsync<TProductResponse>(int page, int size, string? keyword, CancellationToken cancellationToken = default)
        {
            IQueryable<Product> queryable = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLowerInvariant();
                queryable = queryable.Where(product => product.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
            }

            queryable = queryable.OrderBy(product => product.CreatedOn);
            return await queryable.ApplyPagingAsync<Product, TProductResponse>(page, size, cancellationToken);
        }
    }
}
