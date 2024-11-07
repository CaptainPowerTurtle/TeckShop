using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.AutoNSubstitute;
using AutoFixture;
using Catalog.Application.Features.Brands.CreateBrand;
using Catalog.Application.Features.Brands.Dtos;
using ErrorOr;
using Soenneker.Utils.AutoBogus.Config;
using Soenneker.Utils.AutoBogus.Context;
using Soenneker.Utils.AutoBogus.Override;
using Soenneker.Utils.AutoBogus;
using FluentAssertions;
using Catalog.Application.Features.Brands.GetBrand;
using NSubstitute;
using Catalog.Application.Contracts.Caching;
using NSubstitute.ReturnsExtensions;
using Catalog.Domain.Entities.Brands;

namespace Catalog.Application.UnitTests.Brands
{
    public class GetBrandQueryHandlerTests
    {
        //private readonly Substitute<IBrandRepository> _brandRepositoryMock;

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenBrandIsNotNull()
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
        public async Task Handle_Should_ReturnNotFoundResult_WhenBrandIsNullAsync()
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

        public class GetBrandBrandRequestOverride : AutoFakerOverride<CreateBrandRequest>
        {
            public override void Generate(AutoFakerOverrideContext context)
            {
                var target = (context.Instance as CreateBrandRequest)!;

                target.Name = context.Faker.Company.CompanyName();
                target.Description = context.Faker.Company.CatchPhrase();
                target.Website = context.Faker.Internet.Url();
            }
        }
    }
}
