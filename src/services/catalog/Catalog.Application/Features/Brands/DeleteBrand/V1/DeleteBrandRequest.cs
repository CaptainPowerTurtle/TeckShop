﻿namespace Catalog.Application.Features.Brands.DeleteBrand.V1
{
    /// <summary>
    /// The delete brand request.
    /// </summary>
    public sealed record DeleteBrandRequest
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }
    }
}