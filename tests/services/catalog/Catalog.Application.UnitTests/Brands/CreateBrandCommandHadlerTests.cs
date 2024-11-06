using Bogus;
using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Brands.CreateBrand;
using Catalog.Application.Features.Brands.Dtos;
using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using NSubstitute;
using TeckShop.Core.Database;

namespace Catalog.Application.UnitTests.Brands
{
    public class CreateBrandCommandHadlerTests
    {
        //private readonly Substitute<IBrandRepository> _brandRepositoryMock;

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenBrandIsUniqueAsync()
        {
            //Arrange
            var testBrands = new Faker<CreateBrandRequest>()
                .RuleFor(u => u.Name, (f, u) => f.Company.CompanyName())
                .RuleFor(u => u.Description, (f, u) => f.Company.CatchPhrase())
                .RuleFor(u => u.Name, (f, u) => f.Internet.Url());

            var request = testBrands.Generate(1).First();

            var command = new CreateBrand.Command(request);

            var _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var _mapperMock = Substitute.For<IMapper>();
            var _mockBrandCache = Substitute.For<IBrandCache>();
            var _brandRepositoryMock = Substitute.For<IBrandRepository>();

            var handler = new CreateBrand.Handler(_unitOfWorkMock, _mapperMock, _mockBrandCache, _brandRepositoryMock);


            //Act
            ErrorOr<BrandResponse> result = await handler.Handle(command, default);

            //Assert
            result.IsError.Should().BeFalse();
        }
    }
}
