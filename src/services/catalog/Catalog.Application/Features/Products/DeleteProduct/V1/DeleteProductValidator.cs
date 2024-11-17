using Catalog.Application.Contracts.Repositories;
using FastEndpoints;
using FluentValidation;

namespace Catalog.Application.Features.Products.DeleteProduct.V1
{
    /// <summary>
    /// Delete brand validator.
    /// </summary>
    public sealed class DeleteProductValidator : Validator<DeleteProductRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProductValidator"/> class.
        /// </summary>
        public DeleteProductValidator()
        {
            RuleFor(product => product.ProductSKU)
                .NotEmpty()
                .WithName("ProductSKU")
                .MustAsync(async (sku, ct) =>
                {
                    IProductRepository _productRepository = Resolve<IProductRepository>();
                    return await _productRepository.ExistsAsync(product => product.ProductSKU.Equals(sku), cancellationToken: ct);
                })
                .WithMessage((_, productSku) => $"Product with the SKU '{productSku}' does not exists.");
        }
    }
}
