using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Brands.Dtos;
using Catalog.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using TeckShop.Core.Database;

namespace Catalog.Application.Features.Brands.UpdateBrand
{
    /// <summary>
    /// Update brand command.
    /// </summary>
    public sealed record UpdateBrandCommand : IRequest<ErrorOr<BrandResponse>>
    {
        /// <summary>
        /// The id.
        /// </summary>
        internal readonly Guid Id;

        /// <summary>
        /// Name.
        /// </summary>
        internal readonly string? Name;

        /// <summary>
        /// The description.
        /// </summary>
        internal readonly string? Description;

        /// <summary>
        /// The website.
        /// </summary>
        internal readonly string? Website;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBrandCommand"/> class.
        /// </summary>
        /// <param name="updateBrandRequest">The update brand request.</param>
        public UpdateBrandCommand(UpdateBrandRequest updateBrandRequest)
        {
            Id = updateBrandRequest.Id;
            Name = updateBrandRequest.Name;
            Description = updateBrandRequest.Description;
            Website = updateBrandRequest.Website;
        }
    }

    /// <summary>
    /// The handler.
    /// </summary>
    internal sealed class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, ErrorOr<BrandResponse>>
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
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IBrandCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBrandCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="brandRepository">The brand repository.</param>
        public UpdateBrandCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IBrandCache cache, IBrandRepository brandRepository)
        {
            _unitOfWork = unitOfWork;
            _brandRepository = brandRepository;
            _mapper = mapper;
            _cache = cache;
        }

        /// <summary>
        /// Handle and return a task of type erroror.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<ErrorOr<BrandResponse>>]]></returns>
        public async Task<ErrorOr<BrandResponse>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brandToBeUpdated = await _brandRepository.FindByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (brandToBeUpdated == null)
            {
                return Errors.Brand.NotFound;
            }

            brandToBeUpdated.Update(
                request.Name, request.Description, request.Website);

            _brandRepository.Update(brandToBeUpdated);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cache.SetAsync(brandToBeUpdated.Id, brandToBeUpdated, cancellationToken: cancellationToken);

            return _mapper.Map<BrandResponse>(brandToBeUpdated);
        }
    }
}
