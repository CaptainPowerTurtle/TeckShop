using Catalog.Application.Features.Brands.DeleteBrand;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Api.Endpoints.V1.Brands
{
    /// <summary>
    /// The delete brand endpoint.
    /// </summary>
    public class DeleteBrandEndpoint : Endpoint<DeleteBrandRequest, NoContent>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBrandEndpoint"/> class.
        /// </summary>
        /// <param name="mediatr">The mediatr.</param>
        public DeleteBrandEndpoint(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Configure the endpoint.
        /// </summary>
        public override void Configure()
        {
            Delete("/Brands/{Id}");
            Options(ep => ep.RequireProtectedResource("brands", "delete"));
            Version(0);
            Validator<DeleteBrandValidator>();
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(DeleteBrandRequest req, CancellationToken ct)
        {
            var command = new DeleteBrandCommand(req.Id);
            var commandResponse = await _mediatr.Send(command, ct);
            await this.SendNoContentResponseAsync(commandResponse, cancellation: ct);
        }
    }
}
