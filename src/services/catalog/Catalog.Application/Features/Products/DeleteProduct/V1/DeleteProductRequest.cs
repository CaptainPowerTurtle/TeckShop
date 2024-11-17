namespace Catalog.Application.Features.Products.DeleteProduct.V1
{
    /// <summary>
    /// Delete product request.
    /// </summary>
    public sealed record DeleteProductRequest
    {
        /// <summary>
        /// Gets or sets the ProductSKU.
        /// </summary>
        public required string ProductSKU { get; set; }
    }
}
