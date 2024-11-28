using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace TeckShop.Infrastructure.Multitenant
{
    /// <summary>
    /// The keycloak message handler.
    /// </summary>
    public class KeycloakMessageHandler(IConfiguration configuration) : DelegatingHandler
    {
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Sends and return a task of type httpresponsemessage asynchronously.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<HttpResponseMessage>]]></returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Process request");

            AuthToken authToken = await GetAccessTokenAsync(cancellationToken);

            request.Headers.Authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme,
                authToken.AccessToken);

            HttpResponseMessage httpResponseMessage = await base.SendAsync(
                request,
                cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            return httpResponseMessage;
        }

        private async Task<AuthToken> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            KeycloakAuthenticationOptions? keycloakOptions = _configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>() ?? throw new InvalidConfigurationException("Could not authenticate");

            KeyValuePair<string, string>[] reqParams =
            [
                new("client_id", keycloakOptions.Resource),
                new("client_secret", keycloakOptions.Credentials.Secret),
                new("grant_type", "client_credentials")
            ];

            FormUrlEncodedContent content = new(reqParams);

            HttpRequestMessage authRequest = new(
                HttpMethod.Post,
                new Uri(keycloakOptions.KeycloakTokenEndpoint))
            {
                Content = content
            };

            HttpResponseMessage response = await base.SendAsync(authRequest, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AuthToken>(CancellationToken.None) ??
                   throw new InvalidConfigurationException("Could not authenticate");
        }
    }

    internal class Organization
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Alias { get; set; }
        public bool Enabled { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = [];
    }
}
