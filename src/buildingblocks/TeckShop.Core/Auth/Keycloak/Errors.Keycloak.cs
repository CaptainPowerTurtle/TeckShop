using ErrorOr;

namespace TeckShop.Core.Auth.Keycloak
{
    /// <summary>
    /// The errors.
    /// </summary>
    public static partial class Errors
    {
        /// <summary>
        /// The brand.
        /// </summary>
        public static class Organization
        {
            /// <summary>
            /// Gets brand not found error.
            /// </summary>
            public static Error UserNotFound => Error.NotFound(
                code: "Organization.User.NotFound",
                description: "User was not found in the organization");

            /// <summary>
            /// Gets the user data parsing error.
            /// </summary>
            public static Error UserDataParsingError => Error.Unexpected(
                code: "Organization.User.Unexpected",
                description: "User data could not be parsed");
        }
    }
}
