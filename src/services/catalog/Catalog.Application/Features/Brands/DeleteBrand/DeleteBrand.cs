using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Common.Errors;
using ErrorOr;
using TeckShop.Core.CQRS;
using TeckShop.Core.Database;

namespace Catalog.Application.Features.Brands.DeleteBrand
{
    /// <summary>
    /// Delete brand command.
    /// </summary>
    public sealed record DeleteBrandCommand : ICommand<ErrorOr<Deleted>>
    {
        /// <summary>
        /// The id.
        /// </summary>
        public readonly Guid Id;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBrandCommand"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public DeleteBrandCommand(DeleteBrandRequest request)
        {
            Id = request.Id;
        }
    }

    /// <summary>
    /// Delete Brand command handler.
    /// </summary>
    internal sealed class DeleteBrandCommandHandler : ICommandHandler<DeleteBrandCommand, ErrorOr<Deleted>>
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The brand repository.
        /// </summary>
        private readonly IBrandRepository _brandRepository;

        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IBrandCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBrandCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="brandRepository">The brand repository.</param>
        public DeleteBrandCommandHandler(IUnitOfWork unitOfWork, IBrandCache cache, IBrandRepository brandRepository)
        {
            _unitOfWork = unitOfWork;
            _brandRepository = brandRepository;
            _cache = cache;
        }

        /// <summary>
        /// Handle and return a task of type erroror.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<ErrorOr<Deleted>>]]></returns>
        public async Task<ErrorOr<Deleted>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brandToDelete = await _brandRepository.FindOneAsync(brand => brand.Id.Equals(request.Id), true, cancellationToken);

            if (brandToDelete is null)
            {
                return Errors.Brand.NotFound;
            }

            _brandRepository.Delete(brandToDelete);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(request.Id, cancellationToken);

            return Result.Deleted;
        }
    }
}
