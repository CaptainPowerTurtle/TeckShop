using Catalog.Application.Contracts.Repositories;
using FastEndpoints;
using FluentValidation;

namespace Catalog.Application.Features.Products.UpdateProduct.V1
{
    /// <summary>
    /// The create product validator.
    /// </summary>
    public sealed class UpdateProductValidator : Validator<UpdateProductRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProductValidator"/> class.
        /// </summary>
        public UpdateProductValidator()
        {
            RuleFor(product => product.ProductSku)
                .NotEmpty()
                .WithName("ProductSku")
                .MustAsync(async (sku, ct) =>
                {
                    IProductRepository _brandRepository = Resolve<IProductRepository>();
                    return await _brandRepository.ExistsAsync(brand => brand.ProductSKU.Equals(sku, StringComparison.InvariantCultureIgnoreCase), cancellationToken: ct);
                })
                .WithMessage((_, productSku) => $"Product with the SKU '{productSku}' does not Exists.");
        }
    }
}
