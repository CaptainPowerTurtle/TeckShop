using System.Text.RegularExpressions;
using Catalog.Domain.Entities.Brands;
using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Entities.ProductPrices;
using Catalog.Domain.Entities.Promotions;
using TeckShop.Core.Domain;

namespace Catalog.Domain.Entities.Products
{
    /// <summary>
    /// The product.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets the details.
        /// </summary>
        public string? Description { get; private set; } = default!;

        /// <summary>
        /// Gets the slug.
        /// </summary>
        public string Slug { get; private set; } = default!;

        /// <summary>
        /// Gets a value indicating whether active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the product SKU.
        /// </summary>
        public string ProductSKU { get; private set; } = default!;

        /// <summary>
        /// Gets the GTIN.
        /// </summary>
        public string? GTIN { get; private set; } = default!;

        /// <summary>
        /// Gets the Brand id.
        /// </summary>
        public Guid? BrandId { get; private set; }

        /// <summary>
        /// Gets the brand.
        /// </summary>
        public Brand? Brand { get; private set; }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public ICollection<Category> Categories { get; private set; } = [];

        /// <summary>
        /// Gets the product prices.
        /// </summary>
        public ICollection<ProductPrice> ProductPrices { get; private set; } = [];

        /// <summary>
        /// Gets the promotions.
        /// </summary>
        public ICollection<Promotion> Promotions { get; private set; } = [];

        /// <summary>
        /// Update a brand.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="isActive"></param>
        /// <param name="productSku"></param>
        /// <param name="gtin"></param>
        /// <param name="brand"></param>
        /// <param name="categories"></param>
        /// <param name="productPrices"></param>
        /// <param name="promotions"></param>
        /// <returns></returns>
        public Product Update(
        string? name,
        string? description,
        bool isActive,
        string? productSku,
        string? gtin,
        Brand? brand,
        ICollection<Category>? categories,
        ICollection<ProductPrice>? productPrices,
        ICollection<Promotion>? promotions)
        {
            if (name is not null && !Name.Equals(name, StringComparison.Ordinal))
            {
                Name = name;
            }

            if (description is not null && Description?.Equals(description, StringComparison.Ordinal) is not true)
            {
                Description = description;
            }

            if (!isActive.Equals(IsActive))
            {
                IsActive = isActive;
            }

            if (productSku is not null && ProductSKU?.Equals(description, StringComparison.Ordinal) is not true)
            {
                ProductSKU = productSku;
            }

            if (gtin is not null && GTIN?.Equals(description, StringComparison.Ordinal) is not true)
            {
                GTIN = gtin;
            }

            if (brand is not null && Brand?.Id.Equals(brand.Id) is not true)
            {
                Brand = brand;
            }

            if (categories is not null)
            {
                Categories = categories;
            }

            if (productPrices is not null)
            {
                ProductPrices = productPrices;
            }

            if (promotions is not null)
            {
                Promotions = promotions;
            }

            return this;
        }

        /// <summary>
        /// Create a brand.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="productSku"></param>
        /// <param name="gtin"></param>
        /// <param name="categories"></param>
        /// <param name="isActive"></param>
        /// <param name="brand"></param>
        public static Product Create(
            string name,
            string? description,
            string? productSku,
            string? gtin,
            ICollection<Category> categories,
            bool? isActive = false,
            Brand? brand = null)
        {
            Product product = new()
            {
                Name = name!,
                Description = description,
                Slug = GetProductSlug(name!),
                IsActive = isActive ?? false,
                ProductSKU = productSku!,
                GTIN = gtin,
                Categories = categories,
                Brand = brand,
                BrandId = brand?.Id
            };

            ProductCreatedDomainEvent @event = new(product.Id, product.Name);
            product.AddDomainEvent(@event);

            return product;
        }

        /// <summary>
        /// Get product slug.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A string.</returns>
        private static string GetProductSlug(string name)
        {
            name = name.Trim();
            name = name.ToLower();
            name = Regex.Replace(name, "[^a-z0-9]+", "-");
            name = Regex.Replace(name, "--+", "-");
            name = name.Trim('-');
            return name;
        }
    }
}
