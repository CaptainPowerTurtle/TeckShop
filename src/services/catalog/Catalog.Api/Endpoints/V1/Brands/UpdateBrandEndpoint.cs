using Catalog.Application.Features.Brands.Dtos;
using Catalog.Application.Features.Brands.UpdateBrand;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Api.Endpoints.V1.Brands
{
    /// <summary>
    /// The update brand endpoint.
    /// </summary>
    public class UpdateBrandEndpoint : Endpoint<UpdateBrandRequest, BrandResponse>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBrandEndpoint"/> class.
        /// </summary>
        /// <param name="mediatr">The mediatr.</param>
        public UpdateBrandEndpoint(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Configure the endpoint.
        /// </summary>
        public override void Configure()
        {
            Put("/Brands");
            Options(ep => ep.RequireProtectedResource("brands", "update"));
            Version(0);
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(UpdateBrandRequest req, CancellationToken ct)
        {
            var command = new UpdateBrandCommand(req.Id, req.Name, req.Description, req.Website);
            var commandResponse = await _mediatr.Send(command, ct);
            await this.SendNoContentResponseAsync(commandResponse, cancellation: ct);
        }
    }
}
