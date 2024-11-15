using Catalog.Domain.Entities.Products;
using Catalog.Domain.Entities.Promotions;
using TeckShop.Core.Domain;

namespace Catalog.Domain.Entities.Categories
{
    /// <summary>
    /// The category.
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string? Name { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Gets the products.
        /// </summary>
        public ICollection<Product> Products { get; private set; } = [];

        /// <summary>
        /// Gets the promotions.
        /// </summary>
        public ICollection<Promotion> Promotions { get; private set; } = [];

        /// <summary>
        /// Update category.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Category Update(
            string? name,
            string? description)
        {
            if (name is not null && Name?.Equals(name, StringComparison.Ordinal) is not true)
            {
                Name = name;
            }

            if (Description?.Equals(description, StringComparison.Ordinal) is not true)
            {
                Description = description;
            }

            return this;
        }

        /// <summary>
        /// Create category.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Category Create(
            string name,
            string? description)
        {
            Category Category = new()
            {
                Name = name,
                Description = description
            };
            return Category;
        }
    }
}
