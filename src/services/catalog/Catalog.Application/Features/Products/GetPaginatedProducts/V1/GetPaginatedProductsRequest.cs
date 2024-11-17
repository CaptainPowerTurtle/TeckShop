using TeckShop.Core.Pagination;

namespace Catalog.Application.Features.Products.GetPaginatedProducts.V1
{
    /// <summary>
    /// The get paginated products request.
    /// </summary>
    public class GetPaginatedProductsRequest : PaginationParameters
    {
        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        public string? Keyword { get; set; }
    }
}
