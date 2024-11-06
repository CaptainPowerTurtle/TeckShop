using Keycloak.AuthServices.Authentication;
using TeckShop.Core.Options;

namespace TeckShop.Infrastructure.Auth
{
    /// <summary>
    /// The keycloak options.
    /// </summary>
    public class KeycloakOptions : KeycloakAuthenticationOptions, IOptionsRoot
    { }
}
