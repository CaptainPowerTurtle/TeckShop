using Catalog.Application.Features.Brands.Dtos;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Infrastructure.Auth;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Application.Features.Brands.GetBrands.V1
{
    /// <summary>
    /// The get paginated brands endpoint.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetBrandsEndpoint"/> class.
    /// </remarks>
    /// <param name="mediatr">The mediatr.</param>
    public class GetBrandsEndpoint(ISender mediatr) : Endpoint<GetBrandsRequest, IReadOnlyList<BrandResponse>>
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
            Get("/Brands/All");
            Options(ep => ep.RequireProtectedResource("brands", "read"));
            Policy(ep => ep.AddRequirements(new IsTenantMember()));
            Version(1);
            PreProcessor<TenantChecker<GetBrandsRequest>>();
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(GetBrandsRequest req, CancellationToken ct)
        {
            GetBrandsQuery query = new();
            IReadOnlyList<BrandResponse> queryResponse = await _mediatr.Send(query, ct);
            await SendAsync(queryResponse, cancellation: ct);
        }
    }
}
