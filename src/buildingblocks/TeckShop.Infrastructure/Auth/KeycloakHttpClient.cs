using System.Net.Http.Json;
using ErrorOr;
using Keycloak.AuthServices.Sdk.Admin.Models;
using TeckShop.Core.Auth.Keycloak;

namespace TeckShop.Infrastructure.Auth
{
    /// <summary>
    /// The keycloak http client.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="KeycloakHttpClient"/> class.
    /// </remarks>
    /// <param name="httpClient">The http client.</param>
    public class KeycloakHttpClient(HttpClient httpClient) : IKeycloakHttpClient
    {
        /// <summary>
        /// The http client.
        /// </summary>
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// Get organization member by id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<ErrorOr<UserRepresentation>>]]></returns>
        public async Task<ErrorOr<UserRepresentation>> GetOrganizationMemberByIdAsync(string organizationId, string userId, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(new Uri($"{_httpClient.BaseAddress}/organizations/{organizationId}/members/{userId}"), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return response.StatusCode == System.Net.HttpStatusCode.NotFound ? (ErrorOr<UserRepresentation>)Errors.Organization.UserNotFound : (ErrorOr<UserRepresentation>)Error.Unexpected();
            }

            UserRepresentation? result = await response.Content.ReadFromJsonAsync<UserRepresentation>(cancellationToken);

            return result ?? (ErrorOr<UserRepresentation>)Errors.Organization.UserDataParsingError;
        }
    }
}
