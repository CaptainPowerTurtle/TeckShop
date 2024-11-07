using TeckShop.Core.Events;

namespace Catalog.Domain.Entities.Brands
{
    /// <summary>
    /// The brand created domain event.
    /// </summary>
    public sealed class BrandCreatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets the brand id.
        /// </summary>
        public Guid BrandId { get; }

        /// <summary>
        /// Gets the brand name.
        /// </summary>
        public string BrandName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="brandId">The brand id.</param>
        /// <param name="brandName">The brand name.</param>
        public BrandCreatedDomainEvent(Guid brandId, string brandName)
        {
            BrandId = brandId;
            BrandName = brandName;
        }
    }
}
