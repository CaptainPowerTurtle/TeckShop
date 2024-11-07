using Amazon.S3;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Bogus;
using Castle.Core.Resource;
using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Brands.CreateBrand;
using Catalog.Application.Features.Brands.Dtos;
using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using MassTransit;
using NSubstitute;
using Soenneker.Utils.AutoBogus;
using Soenneker.Utils.AutoBogus.Config;
using Soenneker.Utils.AutoBogus.Context;
using Soenneker.Utils.AutoBogus.Generators;
using Soenneker.Utils.AutoBogus.Override;
using TeckShop.Core.Database;

namespace Catalog.Application.UnitTests.Brands
{
    public class CreateBrandCommandHandlerTests
    {
        //private readonly Substitute<IBrandRepository> _brandRepositoryMock;

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenBrandIsUniqueAsync()
        {
            //Arrange
            IFixture fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true});

            CreateBrandCommandHandler sut = fixture.Create<CreateBrandCommandHandler>();

            var optionalConfig = new AutoFakerConfig();
            var autoFaker = new AutoFaker(optionalConfig);
            autoFaker.Config.Overrides = [new CreateBrandRequestOverride()];

            var request = autoFaker.Generate<CreateBrandRequest>();

            var command = new CreateBrandCommand(request);

            //Act
            ErrorOr<BrandResponse> result = await sut.Handle(command, default);

            //Assert
            result.IsError.Should().BeFalse();
        }
    }

    public class CreateBrandRequestOverride : AutoFakerOverride<CreateBrandRequest>
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
