using Catalog.Application.Features.Products.Response;
using FastEndpoints;
using Keycloak.AuthServices.Authorization;
using MediatR;
using TeckShop.Core.Pagination;

namespace Catalog.Application.Features.Products.GetPaginatedProducts.V1
{
    /// <summary>
    /// The get paginated products endpoint.
    /// </summary>
    /// <param name="mediatr">The mediatr.</param>
    public class GetPaginatedProductsEndpoint(ISender mediatr) : Endpoint<GetPaginatedProductsRequest, PagedList<ProductResponse>>
    {
        /// <summary>
        /// The mediatr.
        /// </summary>
        private readonly ISender _mediatr = mediatr;

        /// <inheritdoc/>
        public override void Configure()
        {
            Get("/Products");
            AllowAnonymous();
            Options(ep => ep.RequireProtectedResource("products", "read"));
            Version(1);
            Validator<GetPaginatedProductsValidator>();
        }

        /// <inheritdoc/>
        public override async Task HandleAsync(GetPaginatedProductsRequest req, CancellationToken ct)
        {
            GetPaginatedProductsQuery query = new(req.Page, req.Size, req.NameFilter, req.SortDecending, req.SortValue);
            PagedList<ProductResponse> queryResponse = await _mediatr.Send(query, ct);
            await this.SendAsync(queryResponse, cancellation: ct);
        }
    }
}
