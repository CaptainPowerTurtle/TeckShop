﻿using Catalog.Application.Contracts.Repositories;
using FastEndpoints;
using FluentValidation;

namespace Catalog.Application.Features.Brands.CreateBrand
{
    /// <summary>
    /// The create brand validator.
    /// </summary>
    public sealed class CreateBrandValidator : Validator<CreateBrandRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBrandValidator"/> class.
        /// </summary>
        public CreateBrandValidator()
        {
            RuleFor(brand => brand.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithName("Name")
                .MustAsync(async (name, ct) =>
                {
                    var _brandRepository = Resolve<IBrandRepository>();
                    return !await _brandRepository.ExistsAsync(brand => brand.Name == name, cancellationToken: ct);
                })
                .WithMessage((_, productSku) => $"Brand with the name '{productSku}' already Exists.");
        }
    }
}
