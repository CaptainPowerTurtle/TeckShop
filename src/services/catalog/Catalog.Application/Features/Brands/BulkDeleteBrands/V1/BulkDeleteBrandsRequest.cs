namespace Catalog.Application.Features.Brands.BulkDeleteBrands.V1
{
    /// <summary>
    /// The delete brands request.
    /// </summary>
    public sealed record BulkDeleteBrandsRequest
    {
        /// <summary>
        /// Gets or sets the ids.
        /// </summary>
        public IReadOnlyCollection<Guid> Ids { get; set; } = [];
    }
}
