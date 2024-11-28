using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Brands.Dtos;
using MediatR;

namespace Catalog.Application.Features.Brands.GetBrands.V1
{
    /// <summary>
    /// Get paginated brands query.
    /// </summary>
    public sealed record GetBrandsQuery() : IRequest<IReadOnlyList<BrandResponse>>;

    /// <summary>
    /// Get paginated brands query handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetBrandsQueryHandler"/> class.
    /// </remarks>
    /// <param name="brandRepository">The brand repository.</param>
    internal sealed class GetBrandsQueryHandler(IBrandRepository brandRepository) : IRequestHandler<GetBrandsQuery, IReadOnlyList<BrandResponse>>
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
        public async Task<IReadOnlyList<BrandResponse>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Domain.Entities.Brands.Brand> res = await _brandRepository.GetAllAsync(enableTracking: false, cancellationToken);
            return BrandMappings.BrandListToBrandResponseList(res);
        }
    }
}
