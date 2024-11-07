using Catalog.Application.Features.Brands.Dtos;
using Catalog.Application.Features.Brands.GetBrand;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Api.Endpoints.V1.Brands
{
    /// <summary>
    /// The get brand endpoint.
    /// </summary>
    public class GetBrandEndpoint : Endpoint<GetBrandRequest, BrandResponse>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBrandEndpoint"/> class.
        /// </summary>
        /// <param name="mediatr">The mediatr.</param>
        public GetBrandEndpoint(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Configure the endpoint.
        /// </summary>
        public override void Configure()
        {
            Get("/Brands/{Id}");
            AllowAnonymous();
            Options(ep => ep.RequireProtectedResource("brands", "read"));
            Version(0);
            Validator<GetBrandValidator>();
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(GetBrandRequest req, CancellationToken ct)
        {
            var query = new GetBrandQuery(req.Id);
            var queryResponse = await _mediatr.Send(query, ct);
            await this.SendAsync(queryResponse, ct);
        }
    }
}
