namespace Yarp.Gateway.Services
{
    public interface ICustomSecurityService
    {
        Task<bool> IsUserAllowed(string userId);
        Task<bool> IsUserAllowed(string userId, string projectId);

        Task<string> GetAPIKey(string userId, string projectId);
        Task<bool> IsAPIKeyAllowed(string apiKey);
        Task<bool> IsAPIKeyAllowed(string userId, string projectId);
    }
}