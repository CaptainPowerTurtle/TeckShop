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
        public static class Category
        {
            /// <summary>
            /// Gets brand not found error.
            /// </summary>
            public static Error NotFound => Error.NotFound(
                code: "Category.NotFound",
                description: "No matching category could be found");
        }
    }
}
