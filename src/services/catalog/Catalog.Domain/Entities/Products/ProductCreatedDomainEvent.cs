using TeckShop.Core.Events;

namespace Catalog.Domain.Entities.Products
{
    /// <summary>
    /// The brand created domain event.
    /// </summary>
    public sealed class ProductCreatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets the brand id.
        /// </summary>
        public Guid ProductId { get; }

        /// <summary>
        /// Gets the brand name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="productId">The brand id.</param>
        /// <param name="name">The brand name.</param>
        public ProductCreatedDomainEvent(Guid productId, string name)
        {
            ProductId = productId;
            Name = name;
        }
    }
}
