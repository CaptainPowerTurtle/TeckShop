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
        public static class Brand
        {
            /// <summary>
            /// Gets brand not found error.
            /// </summary>
            public static Error NotFound => Error.NotFound(
                code: "Brand.NotFound",
                description: "The specified brand was not found");
        }
    }
}
