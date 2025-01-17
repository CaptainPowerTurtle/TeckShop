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

            /// <summary>
            /// Gets the organization id not found.
            /// </summary>
            public static Error IdNotFound => Error.NotFound(
                code: "Organization.Id.NotFound",
                description: "Organization Id was not found in the claim");

            /// <summary>
            /// Gets the organization parsing error.
            /// </summary>
            public static Error ParsingError => Error.NotFound(
                code: "Organization.Unexpected",
                description: "Organization claim could not be parsed");

            /// <summary>
            /// Gets the claim not F ound.
            /// </summary>
            public static Error ClaimNotFound => Error.NotFound(
                code: "Organization.Claim.NotFound",
                description: "Organization claim could not be found");
        }
    }
}
