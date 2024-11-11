using Catalog.Application.Features.Products.Response;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Infrastructure.Endpoints;

namespace Catalog.Application.Features.Products.GetProductById.V1
{
    /// <summary>
    /// The get product by id endpoint.
    /// </summary>
    public class GetProductByIdEndpoint : Endpoint<GetProductByIdRequest, ProductResponse>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductByIdEndpoint"/> class.
        /// </summary>
        /// <param name="mediatr">The mediatr.</param>
        public GetProductByIdEndpoint(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Configure the endpoint.
        /// </summary>
        public override void Configure()
        {
            Get("/Products/{Id}");
            AllowAnonymous();
            Options(ep => ep.RequireProtectedResource("products", "read"));
            Version(0);
            Validator<GetProductByIdValidator>();
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(GetProductByIdRequest req, CancellationToken ct)
        {
            var query = new GetProductByIdQuery(req.ProductId);
            var queryResponse = await _mediatr.Send(query, ct);
            await this.SendAsync(queryResponse, ct);
        }
    }
}
