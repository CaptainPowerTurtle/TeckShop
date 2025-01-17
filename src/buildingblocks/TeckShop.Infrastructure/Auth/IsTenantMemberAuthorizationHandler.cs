using System.Security.Claims;
using System.Text.Json;
using ErrorOr;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TeckShop.Core.Auth;
using TeckShop.Core.Auth.Keycloak;
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
            string? organizationClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == "organization")?.Value;

            if (!string.IsNullOrWhiteSpace(organizationClaim))
            {
                ErrorOr<OrganizationRepresentation> organizationRepresentation = GetOrganizationFromClaim(organizationClaim);

                if (!organizationRepresentation.IsError)
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? tenantId = _httpContextAccessor.HttpContext?.Request.Headers[AuthConstants.TenantHeader];

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(tenantId))
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

        private ErrorOr<OrganizationRepresentation> GetOrganizationFromClaim(string organizationClaim)
        {
            try
            {
                // Parse the JSON string (organization claim is expected to be a JSON array)
                JsonElement jsonElement = JsonDocument.Parse(organizationClaim).RootElement;

                if (jsonElement.ValueKind == JsonValueKind.Array)
                {
                    // Handle the array case
                    if (jsonElement.GetArrayLength() > 0)
                    {
                        JsonElement firstElement = jsonElement[0];
                        return ExtractObjectNameAndId(firstElement);
                    }
                }
                else if (jsonElement.ValueKind == JsonValueKind.Object)
                {
                    // Handle the object case
                    return ExtractObjectNameAndId(jsonElement);
                }

                return Errors.Organization.ParsingError;
            }
            catch (JsonException)
            {
                return Errors.Organization.ParsingError;
            }
        }

        private ErrorOr<OrganizationRepresentation> ExtractObjectNameAndId(JsonElement jsonElement)
        {
            JsonProperty property = jsonElement.EnumerateObject().FirstOrDefault();

            OrganizationRepresentation organizationRepresentation = new()
            {
                Name = property.Name // Name of the object (e.g., "Test")
            };

            if (property.Value.TryGetProperty("id", out JsonElement id))
            {
                string? tenantId = id.GetString();

                if (tenantId is null)
                {
                    return Errors.Organization.IdNotFound;
                }

                organizationRepresentation.Id = Guid.Parse(tenantId);
            }

            return organizationRepresentation;
        }
    }
}
