using Catalog.Domain.Entities.Products;
using TeckShop.Core.Database;
using TeckShop.Core.Pagination;

namespace Catalog.Application.Contracts.Repositories
{
    /// <summary>
    /// Brand repository interface.
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product, Guid>
    {
        /// <summary>
        /// Get paged brands async.
        /// </summary>
        /// <typeparam name="TProductResponse"></typeparam>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="keyword"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<PagedList<TProductResponse>> GetPagedProductsAsync<TProductResponse>(int page, int size, string? keyword, CancellationToken cancellationToken = default);
    }
}
