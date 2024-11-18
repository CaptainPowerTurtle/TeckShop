using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Products.Response;
using Catalog.Domain.Common.Errors;
using Catalog.Domain.Entities.Brands;
using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Entities.ProductPrices;
using Catalog.Domain.Entities.Products;
using ErrorOr;
using TeckShop.Core.CQRS;
using TeckShop.Core.Database;

namespace Catalog.Application.Features.Products.UpdateProduct.V1
{
    /// <summary>
    /// The update product command.
    /// </summary>
    public sealed record UpdateProductCommand(Guid ProductId, string Name, string? Description, string? ProductSku, string? GTIN, bool IsActive, Guid? BrandId, IReadOnlyCollection<Guid> Categories) : ICommand<ErrorOr<ProductResponse>>;

    /// <summary>
    /// Create Brand command handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UpdateProductCommand"/> class.
    /// </remarks>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productRepository">The brand repository.</param>
    /// <param name="brandRepository"></param>
    /// <param name="categoryRepository"></param>
    internal sealed class UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository) : ICommandHandler<UpdateProductCommand, ErrorOr<ProductResponse>>
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// The product repository.
        /// </summary>
        private readonly IProductRepository _productRepository = productRepository;

        /// <summary>
        /// The brand repository.
        /// </summary>
        private readonly IBrandRepository _brandRepository = brandRepository;

        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        /// <summary>
        /// Handle and return a task of type erroror.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<ErrorOr<Created>>]]></returns>
        public async Task<ErrorOr<ProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Brand? exisitingBrand = null;
            IReadOnlyList<Category>? categories = [];
            IReadOnlyList<ProductPrice>? productPrices = [];

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

            Product? productToUpdate = await _productRepository.FindOneAsync(product => product.Id.Equals(request.ProductId), true, cancellationToken);

            if (productToUpdate is null)
            {
                return Errors.Product.NotFound;
            }

            productToUpdate.Update(request.Name, request.Description, request.IsActive, request.ProductSku, request.GTIN, exisitingBrand, [.. categories], [.. productPrices], null);

            _productRepository.Update(productToUpdate);

            int result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Equals(0) ? (ErrorOr<ProductResponse>)Errors.Product.NotCreated : (ErrorOr<ProductResponse>)ProductMappings.ProductToProductResponse(productToUpdate);
        }
    }
}
