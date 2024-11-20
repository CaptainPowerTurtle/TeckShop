using Catalog.Application.Features.Brands.Dtos;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Core.Pagination;

namespace Catalog.Application.Features.Brands.GetPaginatedBrands.V1
{
    /// <summary>
    /// The get paginated brands endpoint.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetPaginatedBrandsEndpoint"/> class.
    /// </remarks>
    /// <param name="mediatr">The mediatr.</param>
    public class GetPaginatedBrandsEndpoint(ISender mediatr) : Endpoint<GetPaginatedBrandsRequest, PagedList<BrandResponse>>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr = mediatr;

        /// <summary>
        /// Configure the endpoint.
        /// </summary>
        public override void Configure()
        {
            Get("/Brands");
            Options(ep => ep.RequireProtectedResource("brands", "read"));
            Version(1);
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(GetPaginatedBrandsRequest req, CancellationToken ct)
        {
            GetPaginatedBrandsQuery query = new(req.Page, req.Size, req.NameFilter, req.SortDecending, req.SortValue);
            PagedList<BrandResponse> queryResponse = await _mediatr.Send(query, ct);
            await SendAsync(queryResponse, cancellation: ct);
        }
    }
}
