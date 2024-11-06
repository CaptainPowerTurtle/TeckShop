using NSwag;

namespace TeckShop.Infrastructure.Swagger
{
    /// <summary>
    /// The swagger auth.
    /// </summary>
    public static class SwaggerAuth
    {
        /// <summary>
        /// Add O auth scheme.
        /// </summary>
        /// <param name="tokenUrl">The token url.</param>
        /// <param name="authorizationUrl">The authorization url.</param>
        /// <param name="refreshUrl">The refresh url.</param>
        /// <returns>An OpenApiSecurityScheme.</returns>
        public static OpenApiSecurityScheme AddOAuthScheme(string tokenUrl, string authorizationUrl, string refreshUrl)
        {
            return new OpenApiSecurityScheme()
            {
                In = OpenApiSecurityApiKeyLocation.Header,
                Type = OpenApiSecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = authorizationUrl,
                        TokenUrl = tokenUrl,
                        RefreshUrl = refreshUrl,
                        Scopes = new Dictionary<string, string>
                        {
                                { "openid", "openid" },
                                { "profile", "profile" }
                        }
                    }
                }
            };
        }
    }
}
