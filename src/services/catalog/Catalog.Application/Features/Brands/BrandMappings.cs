using Catalog.Application.Features.Brands.Dtos;
using Catalog.Domain.Entities.Brands;
using Riok.Mapperly.Abstractions;

namespace Catalog.Application.Features.Brands
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    internal static partial class BrandMappings
    {
        internal static partial IReadOnlyList<BrandResponse> BrandListToBrandResponseList(this IReadOnlyList<Brand> brands);
    }
}
