namespace Catalog.Application.Features.Brands.GetBrands.V1
{
    /// <summary>
    /// The get paginated brands request.
    /// </summary>
    public class GetBrandsRequest
    {
        /// <summary>
        /// Gets or sets the name filter.
        /// </summary>
        public string? NameFilter { get; set; }
    }
}
