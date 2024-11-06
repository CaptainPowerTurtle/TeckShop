namespace Yarp.Gateway.Services
{
    public class CustomSecurityService : ICustomSecurityService
    {
        readonly ILogger<CustomSecurityService> _logger;

        public CustomSecurityService(ILogger<CustomSecurityService> logger)
        {
            _logger = logger;
        }

        public Task<string> GetAPIKey(string userId, string projectId)
        {
            _logger.LogError("Not supported");
            throw new NotSupportedException("Get api key not supported");
        }

        public Task<bool> IsAPIKeyAllowed(string apiKey)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsAPIKeyAllowed(string userId, string projectId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsUserAllowed(string userId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsUserAllowed(string userId, string projectId)
        {
            return Task.FromResult(true);
        }
    }
}
