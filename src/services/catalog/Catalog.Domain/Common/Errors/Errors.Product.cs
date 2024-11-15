using ErrorOr;

namespace Catalog.Domain.Common.Errors
{
    /// <summary>
    /// The errors.
    /// </summary>
    public static partial class Errors
    {
        /// <summary>
        /// The brand.
        /// </summary>
        public static class Product
        {
            /// <summary>
            /// Gets brand not found error.
            /// </summary>
            public static Error NotFound => Error.NotFound(
                code: "Product.NotFound",
                description: "The specified product was not found");

            /// <summary>
            /// Gets the not created.
            /// </summary>
            public static Error NotCreated => Error.Failure(
                code: "Product.NotCreated",
                description: "The product was not created");
        }
    }
}
