using TeckShop.Core.Pagination;

namespace Catalog.Application.Features.Brands.GetPaginatedBrands.V1
{
    /// <summary>
    /// The get paginated brands request.
    /// </summary>
    public class GetPaginatedBrandsRequest : PaginationParameters
    {
        /// <summary>
        /// Gets or sets the keyword.
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
