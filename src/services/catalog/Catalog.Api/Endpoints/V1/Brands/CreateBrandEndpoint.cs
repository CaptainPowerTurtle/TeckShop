using Catalog.Application.Features.Brands.CreateBrand;
using Catalog.Application.Features.Brands.Dtos;
using ErrorOr;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Api.Endpoints.V1.Brands
{
    /// <summary>
    /// The create brand endpoint.
    /// </summary>
    public class CreateBrandEndpoint : Endpoint<CreateBrandRequest, BrandResponse>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBrandEndpoint"/> class.
        /// </summary>
        /// <param name="mediatr">The mediatr.</param>
        public CreateBrandEndpoint(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Configure the endpoint.
        /// </summary>
        public override void Configure()
        {
            Post("/Brands");
            Options(ep => ep.RequireProtectedResource("brands", "create")/*.AddEndpointFilter<IdempotentAPIEndpointFilter>()*/);
            Version(0);
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
            var command = new CreateBrandCommand(req.Name, req.Description, req.Website);
            var commandResponse = await _mediatr.Send(command, ct);
            await this.SendCreatedAtAsync<GetBrandEndpoint, ErrorOr<BrandResponse>>(routeValues: new { commandResponse.Value?.Id }, commandResponse, cancellation: ct);
        }
    }
}
