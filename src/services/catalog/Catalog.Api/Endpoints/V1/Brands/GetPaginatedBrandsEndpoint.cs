using Catalog.Application.Features.Brands.Dtos;
using Catalog.Application.Features.Brands.GetPaginatedBrands;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Core.Pagination;

namespace Catalog.Api.Endpoints.V1.Brands
{
    /// <summary>
    /// The get paginated brands endpoint.
    /// </summary>
    public class GetPaginatedBrandsEndpoint : Endpoint<GetPaginatedBrandsRequest, PagedList<BrandResponse>>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedBrandsEndpoint"/> class.
        /// </summary>
        /// <param name="mediatr">The mediatr.</param>
        public GetPaginatedBrandsEndpoint(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Configure the endpoint.
        /// </summary>
        public override void Configure()
        {
            Get("/Brands");
            Options(ep => ep.RequireProtectedResource("brands", "read"));
            Version(0);
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(GetPaginatedBrandsRequest req, CancellationToken ct)
        {
            var query = new GetPaginatedBrandsQuery(req);
            var queryResponse = await _mediatr.Send(query, ct);
            await SendAsync(queryResponse, cancellation: ct);
        }
    }
}
