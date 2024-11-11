using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Entities.Products;
using TeckShop.Core.Domain;

namespace Catalog.Domain.Entities.Promotions
{
    /// <summary>
    /// The promotion.
    /// </summary>
    public class Promotion : BaseEntity
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Gets the valid from.
        /// </summary>
        public DateTimeOffset ValidFrom { get; private set; } = default!;

        /// <summary>
        /// Gets the valid converts to.
        /// </summary>
        public DateTimeOffset ValidTo { get; private set; } = default!;

        /// <summary>
        /// Gets the products.
        /// </summary>
        public ICollection<Product> Products { get; private set; } = [];

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public ICollection<Category> Categories { get; private set; } = [];

        /// <summary>
        /// Update a brand.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="validFrom"></param>
        /// <param name="validTo"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public Promotion Update(
        string? name,
        string? description,
        DateTimeOffset? validFrom,
        DateTimeOffset? validTo,
        ICollection<Product>? products)
        {
            if (name is not null && Name?.Equals(name, StringComparison.Ordinal) is not true)
                Name = name;
            if (Description?.Equals(description, StringComparison.Ordinal) is not true)
                Description = description;
            if (validFrom.HasValue && !ValidFrom.Equals(validFrom))
                ValidFrom = validFrom.Value;
            if (validTo.HasValue && !ValidTo.Equals(validTo))
                ValidTo = validTo.Value;
            if (products is not null)
                Products = products;
            return this;
        }

        /// <summary>
        /// Create a brand.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="validFrom"></param>
        /// <param name="validTo"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public static Promotion Create(
        string name,
        string? description,
        DateTimeOffset validFrom,
        DateTimeOffset validTo,
        ICollection<Product> products)
        {
            Promotion promotion = new()
            {
                Name = name,
                Description = description,
                ValidFrom = validFrom,
                ValidTo = validTo,
                Products = products
            };

            return promotion;
        }
    }
}
