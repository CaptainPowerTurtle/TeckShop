using System.Security.Claims;
using System.Text.Json;
using ErrorOr;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TeckShop.Core.Auth;
using ZiggyCreatures.Caching.Fusion;

namespace TeckShop.Infrastructure.Auth
{
    /// <summary>
    /// The checks if is tenant member authorization handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="IsTenantMemberAuthorizationHandler"/> class.
    /// </remarks>
    /// <param name="keycloakHttpClient"></param>
    /// <param name="fusionCache"></param>
    /// <param name="httpContextAccessor"></param>
    public class IsTenantMemberAuthorizationHandler(
        IKeycloakHttpClient keycloakHttpClient,
        IFusionCache fusionCache,
        IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<IsTenantMember>
    {
        /// <summary>
        /// The client factory.
        /// </summary>
        private readonly IKeycloakHttpClient _keycloakHttpClient = keycloakHttpClient;

        private readonly IFusionCache _fusionCache = fusionCache;

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// Handle the requirement asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="requirement">The requirement.</param>
        /// <returns>A Task.</returns>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsTenantMember requirement)
        {
            string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? tenantId = _httpContextAccessor.HttpContext?.Request.Headers[AuthConstants.TenantHeader];
            if (userId is null || tenantId is null)
            {
                context.Fail();
                return;
            }

            string cacheKey = $"{userId}:{tenantId}";

            ErrorOr<UserRepresentation> result = await _fusionCache.GetOrSetAsync<ErrorOr<UserRepresentation>>(
                cacheKey,
                async (context, ct) =>
                {
                    ErrorOr<UserRepresentation> result = await _keycloakHttpClient.GetOrganizationMemberByIdAsync(tenantId, userId, ct);
                    if (result.IsError)
                    {
                        context.Options.Duration = TimeSpan.FromMinutes(5);
                    }

                    return result;
                },
                token: default);

            if (!result.IsError)
            {
                context.Succeed(requirement);
            }
        }
    }
}
