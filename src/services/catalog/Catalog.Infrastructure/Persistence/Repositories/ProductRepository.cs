using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using TeckShop.Core.Pagination;
using TeckShop.Persistence.Database.EFCore;

namespace Catalog.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// The brand repository.
    /// </summary>
    public class ProductRepository : GenericRepository<Product, Guid>, IProductRepository
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        public ProductRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
            _context = context;
        }

        /// <summary>
        /// Get paged brands asynchronously.
        /// </summary>
        /// <typeparam name="TProductResponse"/>
        /// <param name="page">The page.</param>
        /// <param name="size">The size.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<PagedList<ProductResponse>>]]></returns>
        public async Task<PagedList<TProductResponse>> GetPagedBrandsAsync<TProductResponse>(int page, int size, string? keyword, CancellationToken cancellationToken = default)
        {
            var queryable = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLowerInvariant();
                queryable = queryable.Where(brand => brand.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
            }

            queryable = queryable.OrderBy(brand => brand.CreatedOn);
            return await queryable.ApplyPagingAsync<Product, TProductResponse>(page, size, cancellationToken);
        }
    }
}
