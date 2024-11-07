using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using ErrorOr;
using TeckShop.Core.CQRS;

namespace Catalog.Application.Features.Brands.DeleteBrands
{
    /// <summary>
    /// Delete brands command.
    /// </summary>
    public sealed record DeleteBrandsCommand : ICommand<ErrorOr<Deleted>>
    {
        /// <summary>
        /// Gets or sets the brand ids.
        /// </summary>
        public IReadOnlyCollection<Guid> BrandIds { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBrandsCommand"/> class.
        /// </summary>
        /// <param name="deleteBrandsRequest">The delete brands request.</param>
        public DeleteBrandsCommand(DeleteBrandsRequest deleteBrandsRequest)
        {
            BrandIds = new ReadOnlyCollection<Guid>([.. deleteBrandsRequest.Ids]);
        }
    }

    /// <summary>
    /// Delete brands command handler.
    /// </summary>
    internal sealed class DeleteBrandsCommandHandler : ICommandHandler<DeleteBrandsCommand, ErrorOr<Deleted>>
    {
        /// <summary>
        /// The brand repository.
        /// </summary>
        private readonly IBrandRepository _brandRepository;

        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IBrandCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBrandsCommandHandler"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="brandRepository">The brand repository.</param>
        public DeleteBrandsCommandHandler(IBrandCache cache, IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
            _cache = cache;
        }

        /// <summary>
        /// Handle and return a task of type erroror.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<ErrorOr<Deleted>>]]></returns>
        public async Task<ErrorOr<Deleted>> Handle(DeleteBrandsCommand request, CancellationToken cancellationToken)
        {
            await _brandRepository.ExcecutSoftDeleteAsync(request.BrandIds, cancellationToken);
            foreach (var id in request.BrandIds)
            {
                await _cache.RemoveAsync(id, cancellationToken);
            }

            return Result.Deleted;
        }
    }
}
