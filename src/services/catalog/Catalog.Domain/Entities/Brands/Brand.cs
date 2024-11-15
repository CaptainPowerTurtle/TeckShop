using Catalog.Domain.Entities.Products;
using TeckShop.Core.Domain;

namespace Catalog.Domain.Entities.Brands
{
    /// <summary>
    /// The brand.
    /// </summary>
    public class Brand : BaseEntity
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string? Description { get; private set; } = null!;

        /// <summary>
        /// Gets the website.
        /// </summary>
        public string? Website { get; private set; } = null!;

        /// <summary>
        /// Gets the products.
        /// </summary>
        public ICollection<Product> Products { get; private set; } = [];

        /// <summary>
        /// Update a brand.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="website"></param>
        /// <returns></returns>
        public Brand Update(
        string? name,
        string? description,
        string? website)
        {
            if (name is not null && Name?.Equals(name, StringComparison.Ordinal) is not true)
            {
                Name = name;
            }

            if (Description?.Equals(description, StringComparison.Ordinal) is not true)
            {
                Description = description;
            }

            if (Website?.Equals(website, StringComparison.Ordinal) is not true)
            {
                Website = website;
            }

            return this;
        }

        /// <summary>
        /// Create a brand.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="website"></param>
        /// <returns></returns>
        public static Brand Create(
        string name, string? description, string? website)
        {
            Brand brand = new()
            {
                Name = name,
                Description = description!,
                Website = website
            };

            BrandCreatedDomainEvent @event = new(brand.Id, brand.Name);
            brand.AddDomainEvent(@event);

            return brand;
        }
    }
}
