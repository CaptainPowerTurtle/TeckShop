using System.Net;

using Yarp.Gateway.Services;

namespace Yarp.Gateway.Middlewares
{
    public class MembershipAndThrottlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMembershipAndThrottlingService _membershipService;
        private readonly string UserId = "x-user-id";
        private readonly string ProjectId = "x-project-id";

        private readonly ILogger<MembershipAndThrottlingMiddleware> _logger;

        public MembershipAndThrottlingMiddleware(
            RequestDelegate next,
            IMembershipAndThrottlingService membershipService,
            ILogger<MembershipAndThrottlingMiddleware> logger)
        {
            _next = next;
            _membershipService = membershipService;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //This middleware has for purpose to implement a membership and throttling mechanism
            _logger.LogInformation("");
            if (context.Request.Headers.TryGetValue(UserId, out var userId) && context.Request.Headers.TryGetValue(ProjectId, out var projectId))
            {
                if (await _membershipService.IsUserAllowed(userId!, projectId!))
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
