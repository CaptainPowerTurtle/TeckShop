using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;

namespace TeckShop.Infrastructure.Multitenant
{
    /// <summary>
    /// The multi tenant builder extensions fusion cache.
    /// </summary>
    public static class MultiTenantBuilderExtensionsFusionCache
    {
        /// <summary>
        /// Adds FusionCache to the application.
        /// </summary>
        /// <typeparam name="TTenantInfo"/>
        /// <param name="builder">The builder.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <returns><![CDATA[MultiTenantBuilder<TTenantInfo>]]></returns>
        public static MultiTenantBuilder<TTenantInfo> WithFusionCacheStore<TTenantInfo>(this MultiTenantBuilder<TTenantInfo> builder, TimeSpan? slidingExpiration)
            where TTenantInfo : class, ITenantInfo, new()
        {
            object[] storeParams = slidingExpiration is null ? ["__tenant__"] : ["__tenant__", slidingExpiration];

            return builder.WithStore<FusionCacheStore<TTenantInfo>>(ServiceLifetime.Transient, storeParams);
        }
    }
}
