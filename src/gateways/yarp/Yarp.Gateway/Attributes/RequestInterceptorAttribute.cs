namespace Yarp.Gateway.Attributes
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Filters;

    public class RequestInterceptorAttribute : ActionFilterAttribute
    {
        private readonly string targetApiUrl;

        public RequestInterceptorAttribute(string targetApiUrl)
        {
            this.targetApiUrl = targetApiUrl;
        }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.Request.Method == HttpMethod.Post) // Change as needed
            {
                var originalRequest = actionContext.Request;

                using (var client = new HttpClient())
                {
                    // Forward the original request to the target API
                    var requestMessage = new HttpRequestMessage(originalRequest.Method, targetApiUrl)
                    {
                        Content = new StreamContent(await originalRequest.Content.ReadAsStreamAsync())
                    };

                    // Copy headers from the original request
                    foreach (var header in originalRequest.Headers)
                    {
                        requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }

                    // Send the request to the target API
                    var response = await client.SendAsync(requestMessage);

                    // Set the response from the target API as the response for this action
                    actionContext.Response = actionContext.Request.CreateResponse(response.StatusCode);

                    // Copy headers from the target API response
                    foreach (var header in response.Headers)
                    {
                        actionContext.Response.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }

                    // Copy the content (body) from the target API response
                    var responseContent = await response.Content.ReadAsStreamAsync();
                    await responseContent.CopyToAsync(actionContext.Response.Content);
                }
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }

}
