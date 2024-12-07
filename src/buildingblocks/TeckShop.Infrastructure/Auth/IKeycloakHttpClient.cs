using ErrorOr;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace TeckShop.Infrastructure.Auth
{
    /// <summary>
    /// The IKeycloakHttpClient interface.
    /// </summary>
    public interface IKeycloakHttpClient
    {
        /// <summary>
        /// Get organization by memberid async.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ErrorOr<UserRepresentation>> GetOrganizationMemberByIdAsync(string organizationId, string userId, CancellationToken cancellationToken = default);
    }
}
