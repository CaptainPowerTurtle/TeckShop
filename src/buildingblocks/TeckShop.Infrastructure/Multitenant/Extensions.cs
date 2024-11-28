using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.Stores.DistributedCacheStore;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TeckShop.Infrastructure.Multitenant
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add multitenant extension.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="keycloakAuthenticationOptions">The keycloak authentication options.</param>
        public static void AddMultitenantExtension(this IServiceCollection services, KeycloakAuthenticationOptions? keycloakAuthenticationOptions)
        {
            services.AddTransient<KeycloakMessageHandler>();

            string url = keycloakAuthenticationOptions?.AuthServerUrl + "admin/realms/" + keycloakAuthenticationOptions?.Realm + "/organizations/" + "{__tenant__}";

            if (keycloakAuthenticationOptions is not null)
            {
                services.AddMultiTenant<TeckShopTenant>(config =>
                {
                    config.Events.OnTenantResolveCompleted = async (context) =>
                    {
                        if (context.MultiTenantContext.StoreInfo is null)
                        {
                            return;
                        }

                        if (context.MultiTenantContext.StoreInfo.StoreType != typeof(DistributedCacheStore<TeckShopTenant>) && context.MultiTenantContext?.TenantInfo?.Enabled == true)
                        {
                            IServiceProvider sp = ((HttpContext)context.Context!).RequestServices;
                            IMultiTenantStore<TeckShopTenant>? distributedCacheStore = sp
                                .GetService<IEnumerable<IMultiTenantStore<TeckShopTenant>>>()!
                                .FirstOrDefault(tenant => tenant.GetType() == typeof(FusionCacheStore<TeckShopTenant>));

                            await distributedCacheStore!.TryAddAsync(context.MultiTenantContext.TenantInfo!);
                        }

                        await Task.FromResult(0);
                    };
                })
                    .WithHeaderStrategy("x-tenant-id")
                    .WithFusionCacheStore<TeckShopTenant>(TimeSpan.FromMinutes(60))
                    .WithHttpRemoteStore(url, httpClientBuilder =>
                    {
                        httpClientBuilder.AddHttpMessageHandler<KeycloakMessageHandler>();
                    });
            }
            else
            {
                services.AddMultiTenant<TeckShopTenant>();
            }
        }

        /// <summary>
        /// Use multitenant extension.
        /// </summary>
        /// <param name="app">The app.</param>
        public static void UseMultitenantExtension(this IApplicationBuilder app)
        {
            app.UseMultiTenant();
        }
    }
}
