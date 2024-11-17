using Catalog.Application.Contracts.Caching;
using Catalog.Application.Features.Brands.Dtos;
using Catalog.Domain.Common.Errors;
using Catalog.Domain.Entities.Brands;
using ErrorOr;
using MapsterMapper;
using TeckShop.Core.CQRS;

namespace Catalog.Application.Features.Brands.GetBrand.V1
{
    /// <summary>
    /// Get Brand query.
    /// </summary>
    public sealed record GetBrandQuery(Guid Id) : IQuery<ErrorOr<BrandResponse>>;

    /// <summary>
    /// Get brand query handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetBrandQueryHandler"/> class.
    /// </remarks>
    /// <param name="mapper">The mapper.</param>
    /// <param name="cache">The cache.</param>
    internal sealed class GetBrandQueryHandler(IMapper mapper, IBrandCache cache) : IQueryHandler<GetBrandQuery, ErrorOr<BrandResponse>>
    {
        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IBrandCache _cache = cache;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Handle and return a task of type erroror.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<ErrorOr<BrandResponse>>]]></returns>
        public async Task<ErrorOr<BrandResponse>> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            Brand? brand = await _cache.GetOrSetByIdAsync(request.Id, cancellationToken: cancellationToken);

            return brand == null ? (ErrorOr<BrandResponse>)Errors.Brand.NotFound : (ErrorOr<BrandResponse>)_mapper.Map<BrandResponse>(brand);
        }
    }
}
