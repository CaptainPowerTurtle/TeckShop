using TeckShop.Core.Domain;

namespace Catalog.Domain.Entities.ProductPriceOffers
{
    /// <summary>
    /// The product price offer.
    /// </summary>
    public class ProductPriceOffer : BaseEntity
    {
        /// <summary>
        /// Gets the product price id.
        /// </summary>
        public Guid ProductPriceId { get; private set; }

        /// <summary>
        /// Gets the offer price.
        /// </summary>
        public decimal OfferSalePrice { get; private set; } = default!;

        /// <summary>
        /// Gets the valid from.
        /// </summary>
        public DateTimeOffset ValidFrom { get; private set; } = default!;

        /// <summary>
        /// Gets the valid converts to.
        /// </summary>
        public DateTimeOffset ValidTo { get; private set; } = default!;

        /// <summary>
        /// Update product price offer.
        /// </summary>
        /// <param name="offerSalePrice"></param>
        /// <param name="validFrom"></param>
        /// <param name="validTo"></param>
        /// <returns></returns>
        public ProductPriceOffer Update(
            decimal? offerSalePrice, DateTimeOffset? validFrom, DateTimeOffset? validTo)
        {
            if (offerSalePrice.HasValue && !OfferSalePrice.Equals(offerSalePrice))
                OfferSalePrice = offerSalePrice.Value;
            if (validFrom.HasValue && !ValidFrom.Equals(validFrom))
                ValidFrom = validFrom.Value;
            if (validTo.HasValue && !ValidTo.Equals(validTo))
                ValidTo = validTo.Value;
            return this;
        }

        /// <summary>
        /// Create product price offer.
        /// </summary>
        /// <param name="productPriceId"></param>
        /// <param name="offerPrice"></param>
        /// <param name="validFrom"></param>
        /// <param name="validTo"></param>
        /// <returns></returns>
        public static ProductPriceOffer Create(
            Guid productPriceId,
            decimal offerPrice,
            DateTimeOffset validFrom,
            DateTimeOffset validTo)
        {
            ProductPriceOffer productPriceOffer = new()
            {
                ProductPriceId = productPriceId,
                OfferPrice = offerPrice,
                ValidFrom = validFrom,
                ValidTo = validTo,
            };
            return productPriceOffer;
        }
    }
}
