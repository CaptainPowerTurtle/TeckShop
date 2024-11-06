using System.Text.Json.Serialization;
using IdempotentAPI.Cache.FusionCache.Extensions.DependencyInjection;
using IdempotentAPI.DistributedAccessLock.MadelsonDistributedLock.Extensions.DependencyInjection;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using TeckShop.Infrastructure.Options;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace TeckShop.Infrastructure.Caching
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add caching service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheOptions = services.BindValidateReturn<CachingOptions>(configuration);

            var configOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { cacheOptions.RedisURL! },
                Password = cacheOptions.Password
            };

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheOptions.RedisURL;
                options.ConfigurationOptions = configOptions;
            });
            services.AddFusionCacheStackExchangeRedisBackplane(options =>
            {
                options.Configuration = cacheOptions.RedisURL;
                options.ConfigurationOptions = configOptions;
            });
            services.AddFusionCache()
                .WithSerializer(new FusionCacheSystemTextJsonSerializer(new System.Text.Json.JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles }))
                .WithDefaultEntryOptions(new FusionCacheEntryOptions()
                .SetDuration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.High)
                .SetFailSafe(true, TimeSpan.FromHours(2))
                .SetFactoryTimeouts(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(2)));

            var redicConnection = ConnectionMultiplexer.Connect($"{cacheOptions.RedisURL},password={cacheOptions.Password}");
            services.AddSingleton<IDistributedLockProvider>(_ => new RedisDistributedSynchronizationProvider(redicConnection.GetDatabase()));
            services.AddMadelsonDistributedAccessLock();
            services.AddFusionCacheSystemTextJsonSerializer();
            services.AddIdempotentAPIUsingRegisteredFusionCache();
            return services;
        }
    }
}
