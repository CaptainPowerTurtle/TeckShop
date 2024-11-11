using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Products.Response;
using Catalog.Domain.Common.Errors;
using Catalog.Domain.Entities.Brands;
using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Entities.Products;
using ErrorOr;
using TeckShop.Core.CQRS;
using TeckShop.Core.Database;

namespace Catalog.Application.Features.Products.CreateProduct.V1
{
    /// <summary>
    /// Create brand command.
    /// </summary>
    public sealed record CreateProductCommand(string Name, string? Description, string? ProductSku, string? GTIN, bool IsActive, Guid? BrandId, IReadOnlyCollection<Guid> Categories) : ICommand<ErrorOr<ProductResponse>>;

    /// <summary>
    /// Create Brand command handler.
    /// </summary>
    internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ErrorOr<ProductResponse>>
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

        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProductCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="productRepository">The brand repository.</param>
        /// <param name="brandRepository"></param>
        /// <param name="categoryRepository"></param>
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handle and return a task of type erroror.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<ErrorOr<Created>>]]></returns>
        public async Task<ErrorOr<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Brand? exisitingBrand = null;
            IReadOnlyList<Category> categories = [];

            if (request.BrandId.HasValue)
            {
                exisitingBrand = await _brandRepository.FindByIdAsync(request.BrandId.Value, true, cancellationToken);

                if (exisitingBrand is null)
                {
                    return Errors.Brand.NotFound;
                }
            }

            if (request.Categories.Count > 0)
            {
                categories = await _categoryRepository.FindAsync(category => request.Categories.Contains(category.Id), cancellationToken: cancellationToken);

                if (categories.Count.Equals(0))
                {
                    return Errors.Category.NotFound;
                }
            }

            var productToAdd = Product.Create(request.Name, request.Description, request.ProductSku, request.GTIN, categories.ToList(), request.IsActive, exisitingBrand);

            await _productRepository.AddAsync(productToAdd, cancellationToken);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result.Equals(0))
            {
                return Errors.Product.NotCreated;
            }

            return ProductMappings.ProductToProductResponse(productToAdd);
        }
    }
}
