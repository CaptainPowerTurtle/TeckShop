using Catalog.Application.Features.Brands.Dtos;
using Catalog.Domain.Entities.Brands;
using Mapster;

namespace Catalog.Application.Features.Brands.Mappings
{
    /// <summary>
    /// The brand mappings.
    /// </summary>
    public sealed class BrandMappings : IRegister
    {
        /// <summary>
        /// Register the mapping.
        /// </summary>
        /// <param name="config"></param>
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Brand, BrandResponse>();
        }
    }
}
