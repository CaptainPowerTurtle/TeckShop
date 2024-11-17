using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Common.Errors;
using Catalog.Domain.Entities.Products;
using ErrorOr;
using TeckShop.Core.CQRS;
using TeckShop.Core.Database;

namespace Catalog.Application.Features.Products.DeleteProduct.V1
{
    /// <summary>
    /// Delete product command.
    /// </summary>
    public sealed record DeleteProductCommand(string ProductSku) : ICommand<ErrorOr<Deleted>>;

    /// <summary>
    /// Delete product command handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DeleteProductCommand"/> class.
    /// </remarks>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productRepository">The brand repository.</param>
    /// <param name="productCache"></param>
    internal sealed class DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IProductCache productCache) : ICommandHandler<DeleteProductCommand, ErrorOr<Deleted>>
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// The product repository.
        /// </summary>
        private readonly IProductRepository _productRepository = productRepository;

        private readonly IProductCache _productCache = productCache;

        /// <summary>
        /// Handle and return a task of type erroror.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<ErrorOr<Created>>]]></returns>
        public async Task<ErrorOr<Deleted>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Product? productToDelete = await _productRepository.FindOneAsync(product => product.ProductSKU.ToLower().Equals(request.ProductSku.ToLower()), enableTracking: true, cancellationToken);

            if (productToDelete is null)
            {
                return Errors.Product.NotFound;
            }

            _productRepository.Delete(productToDelete);

            await _productCache.RemoveAsync(productToDelete.Id, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}
