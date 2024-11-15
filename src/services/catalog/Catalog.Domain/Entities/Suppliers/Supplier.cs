using TeckShop.Core.Domain;

namespace Catalog.Domain.Entities.Suppliers
{
    /// <summary>
    /// The product.
    /// </summary>
    public class Supplier : BaseEntity
    {
        /// <summary>
        /// Gets the name of the supplier.
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets the description of the supplier.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Gets the website of the supplier.
        /// </summary>
        public string? Website { get; private set; }

        /// <summary>
        /// Update a brand.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="website"></param>
        /// <returns></returns>
        public Supplier Update(
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
        public static Supplier Create(
        string name, string? description, string? website)
        {
            Supplier supplier = new()
            {
                Name = name,
                Description = description,
                Website = website
            };

            return supplier;
        }
    }
}
