using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Products.Response;
using TeckShop.Core.CQRS;
using TeckShop.Core.Pagination;

namespace Catalog.Application.Features.Products.GetPaginatedProducts.V1
{
    /// <summary>
    /// The get paginated products query.
    /// </summary>
    public sealed record GetPaginatedProductsQuery(int Page, int Size, string? Keyword) : IQuery<PagedList<ProductResponse>>;

    /// <summary>
    /// The get paginated products query handler.
    /// </summary>
    /// <param name="productRepository">The product repository.</param>
    internal sealed class GetPaginatedProductsQueryHandler(IProductRepository productRepository) : IQueryHandler<GetPaginatedProductsQuery, PagedList<ProductResponse>>
    {
        /// <summary>
        /// The product repository.
        /// </summary>
        private readonly IProductRepository _productRepository = productRepository;

        /// <summary>
        /// Handle and return a task of a pagedlist of productresponses.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<PagedList<ProductResponse>>]]></returns>
        public async Task<PagedList<ProductResponse>> Handle(GetPaginatedProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetPagedProductsAsync<ProductResponse>(request.Page, request.Size, request.Keyword, cancellationToken);
        }
    }
}
