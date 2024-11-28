using System.Text.Json.Serialization;

namespace TeckShop.Infrastructure.Multitenant
{
    internal class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonPropertyName("refresh_expires_in")]
        public int RrfreshExpiresIn { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
        [JsonPropertyName("not-before-policy")]
        public int NotBefore { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;
    }
}
