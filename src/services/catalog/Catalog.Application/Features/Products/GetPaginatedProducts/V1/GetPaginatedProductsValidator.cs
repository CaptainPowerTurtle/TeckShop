using FastEndpoints;
using FluentValidation;

namespace Catalog.Application.Features.Products.GetPaginatedProducts.V1
{
    /// <summary>
    /// The get paginated products validator.
    /// </summary>
    public sealed class GetPaginatedProductsValidator : Validator<GetPaginatedProductsRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedProductsValidator"/> class.
        /// </summary>
        public GetPaginatedProductsValidator()
        {
            RuleFor(product => product.Page)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);

            RuleFor(product => product.Size)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }
    }
}
