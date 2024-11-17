using Catalog.Application.Features.Products.CreateProduct.V1;
using ErrorOr;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Application.Features.Products.DeleteProduct.V1
{
    /// <summary>
    /// The delete product endpoint.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateProductEndpoint"/> class.
    /// </remarks>
    /// <param name="mediatr">The mediatr.</param>
    public class DeleteProductEndpoint(ISender mediatr) : Endpoint<DeleteProductRequest, NoContent>
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
            Delete("/Products");
            Options(ep => ep.RequireProtectedResource("products", "delete")/*.AddEndpointFilter<IdempotentAPIEndpointFilter>()*/);
            Version(1);
            Validator<DeleteProductValidator>();
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(DeleteProductRequest req, CancellationToken ct)
        {
            DeleteProductCommand command = new(req.ProductSKU);
            ErrorOr<Deleted> commandResponse = await _mediatr.Send(command, ct);
            await this.SendNoContentResponseAsync(commandResponse, cancellation: ct);
        }
    }
}
