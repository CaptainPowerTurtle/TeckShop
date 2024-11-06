using Catalog.Application.Contracts.Caching;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Features.Brands.Dtos;
using Catalog.Domain.Entities.Brands;
using ErrorOr;
using MapsterMapper;
using TeckShop.Core.CQRS;
using TeckShop.Core.Database;

namespace Catalog.Application.Features.Brands.CreateBrand
{
    /// <summary>
    /// The create brand.
    /// </summary>
    public static class CreateBrand
    {
        /// <summary>
        /// The command.
        /// </summary>
        public sealed record Command : ICommand<ErrorOr<BrandResponse>>
        {
            /// <summary>
            /// Name.
            /// </summary>
            internal readonly string Name;

            /// <summary>
            /// The description.
            /// </summary>
            internal readonly string? Description;

            /// <summary>
            /// The website.
            /// </summary>
            internal readonly string? Website;

            /// <summary>
            /// Initializes a new instance of the <see cref="Command"/> class.
            /// </summary>
            /// <param name="createBrandRequest">The create brand request.</param>
            public Command(CreateBrandRequest createBrandRequest)
            {
                Name = createBrandRequest.Name;
                Description = createBrandRequest.Description;
                Website = createBrandRequest.Website;
            }
        }

        /// <summary>
        /// The handler.
        /// </summary>
        public sealed class Handler : ICommandHandler<Command, ErrorOr<BrandResponse>>
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
            /// The brand cache.
            /// </summary>
            private readonly IBrandCache _brandCache;

            /// <summary>
            /// The mapper.
            /// </summary>
            private readonly IMapper _mapper;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="unitOfWork">The unit of work.</param>
            /// <param name="mapper">The mapper.</param>
            /// <param name="brandCache">The brand cache.</param>
            /// <param name="brandRepository">The brand repository.</param>
            public Handler(IUnitOfWork unitOfWork, IMapper mapper, IBrandCache brandCache, IBrandRepository brandRepository)
            {
                _unitOfWork = unitOfWork;
                _brandRepository = brandRepository;
                _mapper = mapper;
                _brandCache = brandCache;
            }

            /// <summary>
            /// Handle and return a task of type erroror.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns><![CDATA[Task<ErrorOr<BrandResponse>>]]></returns>
            public async Task<ErrorOr<BrandResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var brandToAdd = Brand.Create(
                    request.Name!, request.Description, request.Website);

                await _brandRepository.AddAsync(brandToAdd, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _brandCache.SetAsync(brandToAdd.Id, brandToAdd, cancellationToken);
                return _mapper.Map<BrandResponse>(brandToAdd);
            }
        }
    }
}
