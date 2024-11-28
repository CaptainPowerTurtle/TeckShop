using Finbuckle.MultiTenant.Abstractions;
using ZiggyCreatures.Caching.Fusion;

namespace TeckShop.Infrastructure.Multitenant
{
    /// <summary>
    ///  A store for tenant information that uses FusionCache as the backing store.Note that GetAllAsync is not implemented.
    /// </summary>
    /// <typeparam name="TTenantInfo"> The type of tenant information.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FusionCacheStore{TTenantInfo}"/> class.
    /// </remarks>
    /// <param name="cache"></param>
    /// <param name="keyPrefix"></param>
    /// <param name="slidingExpiration"></param>
    public class FusionCacheStore<TTenantInfo>(IFusionCache cache, string keyPrefix, TimeSpan? slidingExpiration) : IMultiTenantStore<TTenantInfo>
        where TTenantInfo : class, ITenantInfo, new()
    {
        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IFusionCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));

        /// <summary>
        /// The key prefix.
        /// </summary>
        private readonly string _keyPrefix = keyPrefix ?? throw new ArgumentNullException(nameof(keyPrefix));

        /// <summary>
        /// Sliding expiration.
        /// </summary>
        private readonly TimeSpan? _slidingExpiration = slidingExpiration;

        /// <inheritdoc />
        public async Task<bool> TryAddAsync(TTenantInfo tenantInfo)
        {
            FusionCacheEntryOptions options = new()
            {
                Duration = _slidingExpiration ?? TimeSpan.FromSeconds(0)
            };
            await _cache.SetAsync($"{_keyPrefix}id__{tenantInfo.Id}", tenantInfo, options);
            await _cache.SetAsync($"{_keyPrefix}identifier__{tenantInfo.Identifier}", tenantInfo, options);
            return true;
        }

        /// <inheritdoc />
        public async Task<TTenantInfo?> TryGetAsync(string id)
        {
            TTenantInfo? result = await _cache.GetOrDefaultAsync<TTenantInfo?>($"{_keyPrefix}id__{id}");

            // Refresh the identifier version to keep things synced
            if (result != null)
            {
                await _cache.SetAsync($"{_keyPrefix}identifier__{result.Identifier}", result, new FusionCacheEntryOptions()
                {
                    Duration = _slidingExpiration ?? TimeSpan.FromSeconds(0)
                });
            }

            return result;
        }

        /// <summary>
        /// Get the all asynchronously.
        /// </summary>
        /// <exception cref="NotImplementedException">.</exception>
        /// <returns><![CDATA[Task<IEnumerable<TTenantInfo>>]]></returns>
        public Task<IEnumerable<TTenantInfo>> GetAllAsync()
        {
            throw new NotSupportedException("Not implemented");
        }

        /// <inheritdoc />
        public async Task<TTenantInfo?> TryGetByIdentifierAsync(string identifier)
        {
            TTenantInfo? result = await _cache.GetOrDefaultAsync<TTenantInfo?>($"{_keyPrefix}identifier__{identifier}");

            if (result != null)
            {
                await _cache.SetAsync($"{_keyPrefix}identifier__{result.Identifier}", result, new FusionCacheEntryOptions()
                {
                    Duration = _slidingExpiration ?? TimeSpan.FromSeconds(0)
                });
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<bool> TryRemoveAsync(string identifier)
        {
            TTenantInfo? result = await TryGetByIdentifierAsync(identifier);
            if (result == null)
            {
                return false;
            }

            await _cache.RemoveAsync($"{_keyPrefix}id__{result.Id}");
            await _cache.RemoveAsync($"{_keyPrefix}identifier__{result.Identifier}");

            return true;
        }

        /// <inheritdoc />
        public Task<bool> TryUpdateAsync(TTenantInfo tenantInfo)
        {
            // Same as adding for distributed cache.
            return TryAddAsync(tenantInfo);
        }
    }
}
