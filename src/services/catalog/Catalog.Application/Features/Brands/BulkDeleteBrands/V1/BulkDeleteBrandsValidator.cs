using Catalog.Application.Features.Brands.DeleteBrand.V1;
using FastEndpoints;
using FluentValidation;

namespace Catalog.Application.Features.Brands.BulkDeleteBrands.V1
{
    /// <summary>
    /// The delete brands validator.
    /// </summary>
    public sealed class BulkDeleteBrandsValidator : Validator<DeleteBrandRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BulkDeleteBrandsValidator"/> class.
        /// </summary>
        public BulkDeleteBrandsValidator()
        {
            RuleFor(brand => brand.Id)
                .NotEmpty();
        }
    }
}
