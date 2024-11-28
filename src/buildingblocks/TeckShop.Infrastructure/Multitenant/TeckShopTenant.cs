using System.Text.Json.Serialization;
using Finbuckle.MultiTenant.Abstractions;

namespace TeckShop.Infrastructure.Multitenant
{
    /// <summary>
    /// The custom tenant.
    /// </summary>
    public class TeckShopTenant : ITenantInfo
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonPropertyName("alias")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Identifier { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enabled.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
