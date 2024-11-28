namespace Catalog.Application.Features.Products.CreateProduct.V1
{
    /// <summary>
    /// The create product request.
    /// </summary>
    public sealed record CreateProductRequest
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        public string? ProductSku { get; set; }

        /// <summary>
        /// Gets or sets the GTIN.
        /// </summary>
        public string? GTIN { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the brand id.
        /// </summary>
        public Guid? BrandId { get; set; }

        /// <summary>
        /// Gets or sets the category ids.
        /// </summary>
        public IReadOnlyCollection<Guid> CategoryIds { get; set; } = [];

        /// <summary>
        /// Gets or sets the product prices.
        /// </summary>
        public IReadOnlyCollection<CreateProductPriceRequest> ProductPrices { get; set; } = [];
    }

    /// <summary>
    /// The create product price request.
    /// </summary>
    public sealed record CreateProductPriceRequest
    {
        /// <summary>
        /// Gets or sets the sale price.
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        public required string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the product price type id.
        /// </summary>
        public Guid ProductPriceTypeId { get; set; }
    }
}
