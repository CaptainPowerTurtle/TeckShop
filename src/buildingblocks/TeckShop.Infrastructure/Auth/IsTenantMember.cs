using Microsoft.AspNetCore.Authorization;

namespace TeckShop.Infrastructure.Auth
{
    /// <summary>
    /// The checks if is tenant member.
    /// </summary>
    public class IsTenantMember : IAuthorizationRequirement
    {
    }
}
