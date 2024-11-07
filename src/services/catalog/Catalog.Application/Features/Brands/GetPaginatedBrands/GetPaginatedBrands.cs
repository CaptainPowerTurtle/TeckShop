using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Brands.Dtos;
using MediatR;
using TeckShop.Core.Pagination;

namespace Catalog.Application.Features.Brands.GetPaginatedBrands
{
    /// <summary>
    /// Get paginated brands query.
    /// </summary>
    public sealed record GetPaginatedBrandsQuery : IRequest<PagedList<BrandResponse>>
    {
        /// <summary>
        /// The page.
        /// </summary>
        internal readonly int Page;

        /// <summary>
        /// The size.
        /// </summary>
        internal readonly int Size;

        /// <summary>
        /// The keyword.
        /// </summary>
        internal readonly string? Keyword;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedBrandsQuery"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public GetPaginatedBrandsQuery(GetPaginatedBrandsRequest request)
        {
            Page = request.Page;
            Size = request.Size;
            Keyword = request.Keyword;
        }
    }

    /// <summary>
    /// Get paginated brands query handler.
    /// </summary>
    internal sealed class GetPaginatedBrandsQueryHandler : IRequestHandler<GetPaginatedBrandsQuery, PagedList<BrandResponse>>
    {
        /// <summary>
        /// The brand repository.
        /// </summary>
        private readonly IBrandRepository _brandRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedBrandsQueryHandler"/> class.
        /// </summary>
        /// <param name="brandRepository">The brand repository.</param>
        public GetPaginatedBrandsQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        /// <summary>
        /// Handle and return a task of a pagedlist of brandresponses.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<PagedList<BrandResponse>>]]></returns>
        public async Task<PagedList<BrandResponse>> Handle(GetPaginatedBrandsQuery request, CancellationToken cancellationToken)
        {
            return await _brandRepository.GetPagedBrandsAsync<BrandResponse>(request.Page, request.Size, request.Keyword, cancellationToken);
        }
    }
}
