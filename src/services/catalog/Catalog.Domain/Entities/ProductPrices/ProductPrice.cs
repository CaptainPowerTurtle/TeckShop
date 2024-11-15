using Catalog.Domain.Entities.ProductPriceTypes;
using Catalog.Domain.Entities.Products;
using TeckShop.Core.Domain;

namespace Catalog.Domain.Entities.ProductPrices
{
    /// <summary>
    /// The product price.
    /// </summary>
    public class ProductPrice : BaseEntity
    {
        /// <summary>
        /// Gets the product id.
        /// </summary>
        public Guid? ProductId { get; private set; } = null!;

        /// <summary>
        /// Gets the product.
        /// </summary>
        public Product Product { get; private set; } = null!;

        /// <summary>
        /// Gets the sale price without VAT.
        /// </summary>
        public decimal SalePrice { get; private set; }

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        public string? CurrencyCode { get; private set; }

        /// <summary>
        /// Gets the product price type.
        /// </summary>
        public ProductPriceType ProductPriceType { get; private set; } = null!;

        /// <summary>
        /// Gets the product price type id.
        /// </summary>
        public Guid? ProductPriceTypeId { get; private set; } = null!;

        /// <summary>
        /// Update product price.
        /// </summary>
        /// <param name="salePrice"></param>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        public ProductPrice Update(
            decimal? salePrice, string? currencyCode)
        {
            if (salePrice.HasValue && !SalePrice.Equals(salePrice))
            {
                SalePrice = salePrice.Value;
            }

            if (currencyCode is not null && CurrencyCode?.Equals(currencyCode) is not true)
            {
                CurrencyCode = currencyCode;
            }

            return this;
        }

        /// <summary>
        /// Create product price.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="salePrice"></param>
        /// <param name="currencyCode"></param>
        /// <param name="productPriceTypeId"></param>
        /// <returns></returns>
        public static ProductPrice Create(
            Guid productId,
            decimal salePrice,
            string currencyCode,
            Guid productPriceTypeId)
        {
            ProductPrice ProductPrice = new()
            {
                ProductId = productId,
                SalePrice = salePrice,
                CurrencyCode = currencyCode,
                ProductPriceTypeId = productPriceTypeId,
            };
            return ProductPrice;
        }
    }
}
