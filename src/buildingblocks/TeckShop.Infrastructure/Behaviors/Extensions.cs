using MediatR;

namespace TeckShop.Infrastructure.Behaviors
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add the behaviors.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddBehaviors(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionalBehavior<,>));
            return services;
        }
    }
}
