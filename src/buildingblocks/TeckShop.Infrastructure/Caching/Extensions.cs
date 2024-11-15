using System.Text.Json.Serialization;
using IdempotentAPI.Cache.FusionCache.Extensions.DependencyInjection;
using IdempotentAPI.DistributedAccessLock.MadelsonDistributedLock.Extensions.DependencyInjection;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
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
        /// <param name="builder">The builder.</param>
        /// <param name="connectionString"></param>
        /// <param name="webHostEnvironment"></param>
        public static void AddCachingService(this WebApplicationBuilder builder, string connectionString, IWebHostEnvironment webHostEnvironment)
        {
            builder.AddRedisDistributedCache("redis");

            builder.Services
                .AddFusionCache()
                .WithRegisteredDistributedCache()
                .WithBackplane(new RedisBackplane(new RedisBackplaneOptions { Configuration = connectionString }))
                .WithSerializer(new FusionCacheSystemTextJsonSerializer(new System.Text.Json.JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles }))
                .WithDefaultEntryOptions(new FusionCacheEntryOptions()
                .SetDuration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.High)
                .SetFailSafe(true, TimeSpan.FromHours(2))
                .SetFactoryTimeouts(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(2)));

            webHostEnvironment.IsDevelopment();

            ConnectionMultiplexer redicConnection = ConnectionMultiplexer.Connect(connectionString);

            builder.Services.AddSingleton<IDistributedLockProvider>(_ => new RedisDistributedSynchronizationProvider(redicConnection.GetDatabase()));
            builder.Services.AddMadelsonDistributedAccessLock();

            builder.Services.AddFusionCacheSystemTextJsonSerializer();
            builder.Services.AddIdempotentAPIUsingRegisteredFusionCache();

            builder.Services.AddHealthChecks().AddRedis(connectionString);
        }
    }
}
