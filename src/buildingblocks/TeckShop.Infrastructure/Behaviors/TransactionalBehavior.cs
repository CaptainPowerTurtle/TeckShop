using MediatR;
using Microsoft.Extensions.Logging;
using TeckShop.Core.CQRS;
using TeckShop.Core.Database;

namespace TeckShop.Infrastructure.Behaviors
{
    internal sealed class TransactionalBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork,
        ILogger<TransactionalBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, ITransactionalCommand<TResponse>
    {
#pragma warning disable AV1755 // Name of async method should end with Async or TaskAsync
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("Beginning transaction for {RequestName}", typeof(TRequest).Name);

            await unitOfWork.BeginTransactionAsync(cancellationToken);
            TResponse response = await next();

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Committed transaction for {RequestName}", typeof(TRequest).Name);

            return response;
        }
    }
#pragma warning restore AV1755 // Name of async method should end with Async or TaskAsync
}
