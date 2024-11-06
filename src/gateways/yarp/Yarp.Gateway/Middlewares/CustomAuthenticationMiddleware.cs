using System.Net;

using Yarp.Gateway.Services;

namespace Yarp.Gateway.Middlewares
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMembershipAndThrottlingService _membershipService;
        private readonly ICustomSecurityService _securityService;

        private readonly string UserId = "x-user-id";
        private readonly string ProjectId = "x-project-id";

        private readonly ILogger<CustomAuthenticationMiddleware> _logger;

        public CustomAuthenticationMiddleware(
            RequestDelegate next,
            ICustomSecurityService securityService,
            IMembershipAndThrottlingService membershipService,
            ILogger<CustomAuthenticationMiddleware> logger)
        {
            _next = next;
            _securityService = securityService;
            _membershipService = membershipService;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _membershipService.IsRequestUnderThrottlingLimits("", "");
            _logger.LogInformation("");
            //This middleware has for purpose to implement a custom authentication/authorization mechanism
            if (context.Request.Headers.TryGetValue(UserId, out var userId) && context.Request.Headers.TryGetValue(ProjectId, out var projectId))
            {
                if (await _securityService.IsUserAllowed(userId!, projectId!))
                {
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            else
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
