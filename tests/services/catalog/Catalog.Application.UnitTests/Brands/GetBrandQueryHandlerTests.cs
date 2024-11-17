using AutoFixture.AutoNSubstitute;
using AutoFixture;
using Catalog.Application.Features.Brands.Dtos;
using ErrorOr;
using Soenneker.Utils.AutoBogus.Config;
using Soenneker.Utils.AutoBogus;
using FluentAssertions;
using NSubstitute;
using Catalog.Application.Contracts.Caching;
using NSubstitute.ReturnsExtensions;
using Catalog.Domain.Entities.Brands;
using Catalog.Application.Features.Brands.GetBrand.V1;

namespace Catalog.Application.UnitTests.Brands
{
    public class GetBrandQueryHandlerTests
    {
        //private readonly Substitute<IBrandRepository> _brandRepositoryMock;

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenBrandIsNotNull_Async()
        {
            //Arrange
            IFixture fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });

            var optionalConfig = new AutoFakerConfig();
            var autoFaker = new AutoFaker(optionalConfig);

            var expected = autoFaker.Generate<Brand>();
            var request = autoFaker.Generate<GetBrandQuery>();

            fixture.Freeze<IBrandCache>().GetOrSetByIdAsync(expected.Id, false, default).Returns(expected);

            GetBrandQueryHandler sut = fixture.Create<GetBrandQueryHandler>();

            //Act
            ErrorOr<BrandResponse> result = await sut.Handle(request, default);

            //Assert
            result.IsError.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFoundResult_WhenBrandIsNull_Async()
        {
            //Arrange
            IFixture fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });

            var optionalConfig = new AutoFakerConfig();
            var autoFaker = new AutoFaker(optionalConfig);

            var request = autoFaker.Generate<GetBrandQuery>();

            fixture.Freeze<IBrandCache>().GetOrSetByIdAsync(request.Id, false, default).ReturnsNull();

            GetBrandQueryHandler sut = fixture.Create<GetBrandQueryHandler>();

            //Act
            ErrorOr<BrandResponse> result = await sut.Handle(request, default);
            //Assert
            result.IsError.Should().BeTrue();
        }
    }
}
