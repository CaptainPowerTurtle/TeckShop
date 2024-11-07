using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Brands.Dtos;
using MediatR;
using TeckShop.Core.Pagination;

namespace Catalog.Application.Features.Brands.GetPaginatedBrands
{
    /// <summary>
    /// Get paginated brands query.
    /// </summary>
    public sealed record GetPaginatedBrandsQuery(int Page, int Size, string? Keyword) : IRequest<PagedList<BrandResponse>>;

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
