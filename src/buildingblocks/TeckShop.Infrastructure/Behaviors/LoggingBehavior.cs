using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TeckShop.Infrastructure.Behaviors
{
#pragma warning disable AV1755 // Name of async method should end with Async or TaskAsync
    internal sealed class LoggingBehavior<TRequest, TResponse>(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        /// <summary>
        /// Pipeline handler. Perform any additional behavior and await the <paramref name="next" /> delegate as necessary.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <typeparamref name="TResponse" />.</returns>
        public async Task<TResponse> Handle(
            TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation($"[START] Handle request={{Request}} - Response={{Response}} - RequestData={{RequestData}}", typeof(TRequest).Name, typeof(TResponse).Name, request);

            Stopwatch timer = new();
            timer.Start();

            TResponse? response = await next();

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)
            {
                logger.LogWarning($"[PERFORMANCE] The request {{Request}} took {{TimeTaken}} seconds.", typeof(TRequest).Name, timeTaken.Seconds);
            }

            logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
            return response;
        }
    }
#pragma warning restore AV1755 // Name of async method should end with Async or TaskAsync
}
