using Catalog.Application.Features.Brands.Dtos;
using ErrorOr;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Application.Features.Brands.UpdateBrand.V1
{
    /// <summary>
    /// The update brand endpoint.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UpdateBrandEndpoint"/> class.
    /// </remarks>
    /// <param name="mediatr">The mediatr.</param>
    public class UpdateBrandEndpoint(ISender mediatr) : Endpoint<UpdateBrandRequest, BrandResponse>
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
            Put("/Brands");
            Options(ep => ep.RequireProtectedResource("brands", "update"));
            PreProcessor<TenantChecker<UpdateBrandRequest>>();
            Version(1);
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(UpdateBrandRequest req, CancellationToken ct)
        {
            UpdateBrandCommand command = new(req.Id, req.Name, req.Description, req.Website);
            ErrorOr<BrandResponse> commandResponse = await _mediatr.Send(command, ct);
            await this.SendNoContentResponseAsync(commandResponse, cancellation: ct);
        }
    }
}
