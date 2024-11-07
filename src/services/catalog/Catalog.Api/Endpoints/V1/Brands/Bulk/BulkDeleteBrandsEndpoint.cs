using Catalog.Application.Features.Brands.DeleteBrands;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Api.Endpoints.V1.Brands.Bulk
{
    /// <summary>
    /// The bulk delete brands endpoint.
    /// </summary>
    public class BulkDeleteBrandsEndpoint : Endpoint<DeleteBrandsRequest, NoContent>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkDeleteBrandsEndpoint"/> class.
        /// </summary>
        /// <param name="mediatr">The mediatr.</param>
        public BulkDeleteBrandsEndpoint(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Configure the endpoint.
        /// </summary>
        public override void Configure()
        {
            Post("/Brands/bulk/delete");
            Options(ep => ep.RequireProtectedResource("brands", "update"));
            Version(0);
            Summary(ep =>
            {
                ep.Summary = "Bulk delete brands";
            });
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(DeleteBrandsRequest req, CancellationToken ct)
        {
            var command = new DeleteBrandsCommand(req);
            var commandResponse = await _mediatr.Send(command, ct);
            await this.SendNoContentResponseAsync(commandResponse, cancellation: ct);
        }
    }
}
