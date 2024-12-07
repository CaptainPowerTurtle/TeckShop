using FastEndpoints;
using Finbuckle.MultiTenant;
using TeckShop.Core.Auth;
using TeckShop.Infrastructure.Multitenant;

namespace TeckShop.Infrastructure.Endpoints
{
/// <summary>
/// The tenant checker.
/// </summary>
/// <typeparam name="TRequest"/>
    public class TenantChecker<TRequest> : IPreProcessor<TRequest>
    {
        /// <summary>
        /// Pres the process asynchronously.
        /// </summary>
        /// <param name="context">The ctx.</param>
        /// <param name="ct">The ct.</param>
        /// <returns>A Task.</returns>
        public async Task PreProcessAsync(IPreProcessorContext<TRequest> context, CancellationToken ct)
        {
            Microsoft.Extensions.Primitives.StringValues tID = context.HttpContext.Request.Headers[AuthConstants.TenantHeader];
            TeckShopTenant? tenant = context.HttpContext.GetMultiTenantContext<TeckShopTenant>()?.TenantInfo;
            if (tID.Count == 0 || tID[0] is null)
            {
                context.ValidationFailures.Add(new("Tenant.NotFound", "The tenant header is missing"));
            }
            else if (tenant == null)
            {
                context.ValidationFailures.Add(new("Tenant.NotFound", "The current tenant was not found"));
            }
            else if (!tenant.Enabled)
            {
                context.ValidationFailures.Add(new("Tenant.Inactive", "The current tenant is not active"));
            }

            if (context.ValidationFailures.Count > 0 && !context.HttpContext.ResponseStarted())
            {
                await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, cancellation: ct);
            }
        }
    }
}
