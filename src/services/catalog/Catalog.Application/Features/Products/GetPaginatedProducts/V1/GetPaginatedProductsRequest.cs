using TeckShop.Core.Pagination;

namespace Catalog.Application.Features.Products.GetPaginatedProducts.V1
{
    /// <summary>
    /// The get paginated products request.
    /// </summary>
    public class GetPaginatedProductsRequest : PaginationParameters
    {
        /// <summary>
        /// Gets or sets the name filter.
        /// </summary>
        public string? NameFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to sort decending.
        /// </summary>
        public bool? SortDecending { get; set; }

        /// <summary>
        /// Gets or sets the sort value.
        /// </summary>
        public string? SortValue { get; set; }
    }
}
