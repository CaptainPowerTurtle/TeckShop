using Yarp.Gateway.Model;

namespace Yarp.Gateway.Services
{
    public class MembershipAndThrottlingService : IMembershipAndThrottlingService
    {
        readonly ILogger<MembershipAndThrottlingService> _logger;

        public MembershipAndThrottlingService(ILogger<MembershipAndThrottlingService> logger)
        {
            _logger = logger;
        }

        public Task<UserMembershipInfo> GetMembershipInfo(string userId)
        {
            _logger.LogError("Not supported");
            throw new NotSupportedException("Not supported");
        }

        public Task<ThrottlingLevels> GetUserThrottlingLevels(string userId, string projectId)
        {
            throw new NotSupportedException("Not supported");
        }

        public Task<bool> IsRequestUnderThrottlingLimits(string userId, string projectId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsUserAllowed(string userId, string projectId)
        {
            return Task.FromResult(true);
        }
    }
}
