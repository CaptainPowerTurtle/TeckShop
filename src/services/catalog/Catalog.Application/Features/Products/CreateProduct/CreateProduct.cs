using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Common.Errors;
using Catalog.Domain.Entities.Brands;
using Catalog.Domain.Entities.Products;
using ErrorOr;
using TeckShop.Core.CQRS;
using TeckShop.Core.Database;

namespace Catalog.Application.Features.Products.CreateProduct
{
    /// <summary>
    /// Create brand command.
    /// </summary>
    public sealed record CreateProductCommand(string Name, string? Description, string? ProductSku, string? GTIN, bool IsActive, Guid? BrandId) : ICommand<ErrorOr<Created>>;

    /// <summary>
    /// Create Brand command handler.
    /// </summary>
    internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ErrorOr<Created>>
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The product repository.
        /// </summary>
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// The brand repository.
        /// </summary>
        private readonly IBrandRepository _brandRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProductCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="productRepository">The brand repository.</param>
        /// <param name="brandRepository"></param>
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IBrandRepository brandRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _brandRepository = brandRepository;
        }

        /// <summary>
        /// Handle and return a task of type erroror.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<ErrorOr<Created>>]]></returns>
        public async Task<ErrorOr<Created>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Brand? exisitingBrand = null;
            if (request.BrandId.HasValue)
            {
                exisitingBrand = await _brandRepository.FindByIdAsync(request.BrandId.Value, true, cancellationToken);

                if (exisitingBrand is null)
                {
                    return Errors.Brand.NotFound;
                }
            }

            var productToAdd = Product.Create(request.Name, request.Description, request.ProductSku, request.GTIN, request.IsActive, exisitingBrand);

            await _productRepository.AddAsync(productToAdd, cancellationToken);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result.Equals(0))
            {
                return Errors.Product.NotCreated;
            }

            return Result.Created;
        }
    }
}
