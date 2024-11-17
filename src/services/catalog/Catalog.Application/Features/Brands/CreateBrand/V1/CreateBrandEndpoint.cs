using Catalog.Application.Features.Brands.Dtos;
using Catalog.Application.Features.Brands.GetBrand.V1;
using ErrorOr;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Application.Features.Brands.CreateBrand.V1
{
    /// <summary>
    /// The create brand endpoint.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateBrandEndpoint"/> class.
    /// </remarks>
    /// <param name="mediatr">The mediatr.</param>
    public class CreateBrandEndpoint(ISender mediatr) : Endpoint<CreateBrandRequest, BrandResponse>
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
            Post("/Brands");
            Options(ep => ep.RequireProtectedResource("brands", "create")/*.AddEndpointFilter<IdempotentAPIEndpointFilter>()*/);
            Version(1);
            Validator<CreateBrandValidator>();
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(CreateBrandRequest req, CancellationToken ct)
        {
            CreateBrandCommand command = new(req.Name, req.Description, req.Website);
            ErrorOr<BrandResponse> commandResponse = await _mediatr.Send(command, ct);
            await this.SendCreatedAtAsync<GetBrandEndpoint, ErrorOr<BrandResponse>>(routeValues: new { commandResponse.Value?.Id }, commandResponse, cancellation: ct);
        }
    }
}