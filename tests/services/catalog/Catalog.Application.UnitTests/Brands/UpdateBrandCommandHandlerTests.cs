using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.AutoNSubstitute;
using AutoFixture;
using Catalog.Application.Features.Brands.CreateBrand;
using Catalog.Application.Features.Brands.Dtos;
using Catalog.Application.UnitTests.Brands;
using ErrorOr;
using Soenneker.Utils.AutoBogus.Config;
using Soenneker.Utils.AutoBogus;
using FluentAssertions;
using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Catalog.Application.Features.Brands.UpdateBrand.V1;

namespace Catalog.Application.UnitTests.Brands
{
    public class UpdateBrandCommandHandlerTests
    {
        //private readonly Substitute<IBrandRepository> _brandRepositoryMock;

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenBrandIsUpdated_Async()
        {
            //Arrange
            IFixture fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });

            UpdateBrandCommandHandler sut = fixture.Create<UpdateBrandCommandHandler>();

            var optionalConfig = new AutoFakerConfig();
            var autoFaker = new AutoFaker(optionalConfig);
            autoFaker.Config.Overrides = [new CreateBrandRequestOverride()];

            var command = autoFaker.Generate<UpdateBrandCommand>();

            //Act
            ErrorOr<BrandResponse> result = await sut.Handle(command, default);

            //Assert
            result.IsError.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFoundResult_WhenBrandIsNotFound_Async()
        {
            //Arrange
            IFixture fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });

            var optionalConfig = new AutoFakerConfig();
            var autoFaker = new AutoFaker(optionalConfig);
            autoFaker.Config.Overrides = [new CreateBrandRequestOverride()];


            var expected = autoFaker.Generate<BrandResponse>();

            fixture.Freeze<IBrandRepository>().FindByIdAsync(expected.Id, false, default).ReturnsNullForAnyArgs();

            UpdateBrandCommandHandler sut = fixture.Create<UpdateBrandCommandHandler>();

            var command = autoFaker.Generate<UpdateBrandCommand>();

            //Act
            ErrorOr<BrandResponse> result = await sut.Handle(command, default);

            //Assert
            result.IsError.Should().BeTrue();
        }
    }
}
