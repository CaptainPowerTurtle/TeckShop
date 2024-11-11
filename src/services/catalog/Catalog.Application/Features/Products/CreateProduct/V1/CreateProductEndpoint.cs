using Catalog.Application.Features.Products.GetProductById.V1;
using Catalog.Application.Features.Products.Response;
using ErrorOr;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Application.Features.Products.CreateProduct.V1
{
    /// <summary>
    /// The create product endpoint.
    /// </summary>
    public class CreateProductEndpoint : Endpoint<CreateProductRequest, ProductResponse>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProductEndpoint"/> class.
        /// </summary>
        /// <param name="mediatr">The mediatr.</param>
        public CreateProductEndpoint(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Configure the endpoint.
        /// </summary>
        public override void Configure()
        {
            Post("/Products");
            Options(ep => ep.RequireProtectedResource("products", "create")/*.AddEndpointFilter<IdempotentAPIEndpointFilter>()*/);
            Version(0);
            Validator<CreateProductValidator>();
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
        {
            var command = new CreateProductCommand(req.Name, req.Description, req.ProductSku, req.GTIN, req.IsActive, req.BrandId, req.CategoryIds);
            var commandResponse = await _mediatr.Send(command, ct);
            await this.SendCreatedAtAsync<GetProductByIdEndpoint, ErrorOr<ProductResponse>>(routeValues: new { commandResponse.Value?.Id }, commandResponse, cancellation: ct);
        }
    }
}
