using Catalog.Application.Contracts.Caching;
using Catalog.Application.Features.Brands.Dtos;
using Catalog.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using TeckShop.Core.CQRS;

namespace Catalog.Application.Features.Brands.GetBrand
{
    /// <summary>
    /// The get brand.
    /// </summary>
    public static class GetBrand
    {
        /// <summary>
        /// The query.
        /// </summary>
        public sealed record Query : IQuery<ErrorOr<BrandResponse>>
        {
            /// <summary>
            /// The id.
            /// </summary>
            public readonly Guid Id;

            /// <summary>
            /// Initializes a new instance of the <see cref="Query"/> class.
            /// </summary>
            /// <param name="request">The request.</param>
            public Query(GetBrandRequest request)
            {
                Id = request.Id;
            }
        }

        /// <summary>
        /// The handler.
        /// </summary>
        public sealed class Handler : IQueryHandler<Query, ErrorOr<BrandResponse>>
        {
            /// <summary>
            /// The cache.
            /// </summary>
            private readonly IBrandCache _cache;

            /// <summary>
            /// The mapper.
            /// </summary>
            private readonly IMapper _mapper;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="mapper">The mapper.</param>
            /// <param name="cache">The cache.</param>
            public Handler(IMapper mapper, IBrandCache cache)
            {
                _mapper = mapper;
                _cache = cache;
            }

            /// <summary>
            /// Handle and return a task of type erroror.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns><![CDATA[Task<ErrorOr<BrandResponse>>]]></returns>
            public async Task<ErrorOr<BrandResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var brand = await _cache.GetOrSetByIdAsync(request.Id, cancellationToken: cancellationToken);

                if (brand == null)
                {
                    return Errors.Brand.NotFound;
                }

                return _mapper.Map<BrandResponse>(brand);
            }
        }
    }
}
