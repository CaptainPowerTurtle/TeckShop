namespace TeckShop.Infrastructure.Multitenant
{
    /// <summary>
    /// The keycloak message handler.
    /// </summary>
    public class KeycloakMessageHandler() : DelegatingHandler
    {
        /// <summary>
        /// Sends and return a task of type httpresponsemessage asynchronously.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<HttpResponseMessage>]]></returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponseMessage = await base.SendAsync(
                request,
                cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            return httpResponseMessage;
        }
    }
}
