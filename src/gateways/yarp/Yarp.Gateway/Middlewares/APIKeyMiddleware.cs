using System.Net;

using Yarp.Gateway.Services;

namespace Yarp.Gateway.Middlewares
{
    public class APIKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMembershipAndThrottlingService _membershipService;
        private readonly ICustomSecurityService _securityService;
        private const string ApiKey = "x-api-key";

        private readonly ILogger<APIKeyMiddleware> _logger;

        public APIKeyMiddleware(
            RequestDelegate next,
            ICustomSecurityService securityService,
            IMembershipAndThrottlingService membershipService,
            ILogger<APIKeyMiddleware> logger)
        {
            _next = next;
            _securityService = securityService;
            _membershipService = membershipService;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _membershipService.GetMembershipInfo("");
            _logger.LogInformation("");
            //This middleware has for purpose to check the prosence and validity of an API Key
            if (context.Request.Headers.TryGetValue(ApiKey, out var requestApiKey))
            {
                if (await _securityService.IsAPIKeyAllowed(requestApiKey!))
                {
                    await _next(context);
                }
                else
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            }
            else
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
