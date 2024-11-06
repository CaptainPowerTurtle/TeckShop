using Yarp.Gateway.Model;

namespace Yarp.Gateway.Services
{
    public interface IMembershipAndThrottlingService
    {
        Task<bool> IsUserAllowed(string userId, string projectId);

        Task<ThrottlingLevels> GetUserThrottlingLevels(string userId, string projectId);

        Task<bool> IsRequestUnderThrottlingLimits(string userId, string projectId);

        Task<UserMembershipInfo> GetMembershipInfo(string userId);
    }
}
