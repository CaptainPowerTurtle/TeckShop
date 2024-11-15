using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Entities.Brands;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TeckShop.Core.Pagination;
using TeckShop.Persistence.Database.EFCore;

namespace Catalog.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// The brand repository.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BrandRepository"/> class.
    /// </remarks>
    /// <param name="context">The context.</param>
    /// <param name="httpContextAccessor">The http context accessor.</param>
    public class BrandRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : GenericRepository<Brand, Guid>(context, httpContextAccessor), IBrandRepository
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly AppDbContext _context = context;

        /// <summary>
        /// Get paged brands asynchronously.
        /// </summary>
        /// <typeparam name="TBrandResponse"/>
        /// <param name="page">The page.</param>
        /// <param name="size">The size.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<PagedList<BrandResponse>>]]></returns>
        public async Task<PagedList<TBrandResponse>> GetPagedBrandsAsync<TBrandResponse>(int page, int size, string? keyword, CancellationToken cancellationToken = default)
        {
            IQueryable<Brand> queryable = _context.Brands.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLowerInvariant();
                queryable = queryable.Where(brand => brand.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
            }

            queryable = queryable.OrderBy(brand => brand.CreatedOn);
            return await queryable.ApplyPagingAsync<Brand, TBrandResponse>(page, size, cancellationToken);
        }

        /// <summary>
        /// Deletes the brands asynchronously.
        /// </summary>
        /// <param name="Ids">The ids.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task.</returns>
        public async Task DeleteBrandsAsync(ICollection<Guid> Ids, CancellationToken cancellationToken = default)
        {
            IQueryable<Brand> queryable = _context.Brands.AsQueryable();
            await queryable.Where(existingBrand => Ids.Any(brandInput => existingBrand.Id.Equals(brandInput))).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
