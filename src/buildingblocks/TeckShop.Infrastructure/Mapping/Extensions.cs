using System.Reflection;
using Mapster;
using MapsterMapper;

namespace TeckShop.Infrastructure.Mapping
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add mapster extension.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="coreAssembly">The core assembly.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddMapsterExtension(this IServiceCollection services, Assembly coreAssembly)
        {
            TypeAdapterConfig typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(coreAssembly);
            Mapper mapperConfig = new(typeAdapterConfig);
            services.AddSingleton<IMapper>(mapperConfig);
            return services;
        }
    }
}
