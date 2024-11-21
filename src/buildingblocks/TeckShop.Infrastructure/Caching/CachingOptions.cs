using TeckShop.Core.Options;

namespace TeckShop.Infrastructure.Caching
{
    /// <summary>
    /// The caching options.
    /// </summary>
    public class CachingOptions : IOptionsRoot
    {
        /// <summary>
        /// Gets or sets the redis URL.
        /// </summary>
        public string? RedisURL { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string? Password { get; set; }
    }
}