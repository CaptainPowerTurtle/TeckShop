using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Brands.Dtos;
using MediatR;
using TeckShop.Core.Pagination;

namespace Catalog.Application.Features.Brands.GetPaginatedBrands.V1
{
    /// <summary>
    /// Get paginated brands query.
    /// </summary>
    public sealed record GetPaginatedBrandsQuery(int Page, int Size, string? NameFilter, bool? SortDecending, string? SortValue) : IRequest<PagedList<BrandResponse>>;

    /// <summary>
    /// Get paginated brands query handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetPaginatedBrandsQueryHandler"/> class.
    /// </remarks>
    /// <param name="brandRepository">The brand repository.</param>
    internal sealed class GetPaginatedBrandsQueryHandler(IBrandRepository brandRepository) : IRequestHandler<GetPaginatedBrandsQuery, PagedList<BrandResponse>>
    {
        /// <summary>
        /// The brand repository.
        /// </summary>
        private readonly IBrandRepository _brandRepository = brandRepository;

        /// <summary>
        /// Handle and return a task of a pagedlist of brandresponses.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<PagedList<BrandResponse>>]]></returns>
        public async Task<PagedList<BrandResponse>> Handle(GetPaginatedBrandsQuery request, CancellationToken cancellationToken)
        {
            return await _brandRepository.GetPagedBrandsAsync<BrandResponse>(request.Page, request.Size, request.NameFilter, request.SortDecending, request.SortValue, cancellationToken);
        }
    }
}