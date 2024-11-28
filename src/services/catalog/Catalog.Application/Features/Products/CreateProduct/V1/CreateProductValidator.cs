using Catalog.Application.Contracts.Repositories;
using FastEndpoints;
using FluentValidation;

namespace Catalog.Application.Features.Products.CreateProduct.V1
{
    /// <summary>
    /// The create product validator.
    /// </summary>
    public sealed class CreateProductValidator : Validator<CreateProductRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProductValidator"/> class.
        /// </summary>
        public CreateProductValidator()
        {
            RuleFor(product => product.ProductSku)
                .NotEmpty()
                .WithName("ProductSku")
                .MustAsync(async (sku, ct) =>
                {
                    IProductRepository _brandRepository = Resolve<IProductRepository>();
                    return !await _brandRepository.ExistsAsync(brand => brand.ProductSKU.Equals(sku), cancellationToken: ct);
                })
                .WithMessage((_, productSku) => $"Product with the SKU '{productSku}' already Exists.");
            RuleFor(product => product.Name)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(255);
        }
    }
}
