using System.Data;
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
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("Beginning transaction for {RequestName}", typeof(TRequest).Name);

            using IDbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
            TResponse response = await next();

            transaction.Commit();

            logger.LogInformation("Committed transaction for {RequestName}", typeof(TRequest).Name);

            return response;
        }
    }
}
