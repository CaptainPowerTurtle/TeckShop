using Catalog.Domain.Entities.Brands;
using TeckShop.Core.Database;
using TeckShop.Core.Pagination;

namespace Catalog.Application.Contracts.Repositories
{
    /// <summary>
    /// Brand repository interface.
    /// </summary>
    public interface IBrandRepository : IGenericRepository<Brand, Guid>
    {
        /// <summary>
        /// Get paged brands async.
        /// </summary>
        /// <typeparam name="TBrandResponse"></typeparam>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="keyword"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<PagedList<TBrandResponse>> GetPagedBrandsAsync<TBrandResponse>(int page, int size, string? keyword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete brands async.
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task DeleteBrandsAsync(ICollection<Guid> Ids, CancellationToken cancellationToken = default);
    }
}
