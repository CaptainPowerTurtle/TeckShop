using Yarp.Gateway.Services;

namespace Yarp.Gateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddSingleton<ICustomSecurityService, CustomSecurityService>();
            services.AddSingleton<IMembershipAndThrottlingService, MembershipAndThrottlingService>();

            return services;
        }
    }
}
