using Catalog.Domain.Entities.ProductPrices;
using TeckShop.Core.Domain;

namespace Catalog.Domain.Entities.ProductPriceTypes
{
    /// <summary>
    /// The product price type.
    /// </summary>
    public class ProductPriceType : BaseEntity
    {
        /// <summary>
        /// Gets the product prices.
        /// </summary>
        public ICollection<ProductPrice> ProductPrices { get; private set; } = [];

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets the index.
        /// </summary>
        public int Priority { get; private set; } = default!;

        /// <summary>
        /// Update Product Price Type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public ProductPriceType Update(
            string? name, int? priority)
        {
            if (name is not null && !Name.Equals(name))
                Name = name;
            if (priority.HasValue && !Priority.Equals(priority))
                Priority = priority.Value;
            return this;
        }

        /// <summary>
        /// Create Product Price Type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static ProductPriceType Create(
        string name, int priority)
        {
            ProductPriceType productPriceType = new()
            {
                Name = name,
                Priority = priority
            };
            return productPriceType;
        }
    }
}
