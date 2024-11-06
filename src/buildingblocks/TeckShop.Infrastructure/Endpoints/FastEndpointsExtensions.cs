using ErrorOr;
using FastEndpoints;
using FluentValidation.Results;

namespace TeckShop.Infrastructure.Endpoints
{
    /// <summary>
    /// The fast endpoints extensions.
    /// </summary>
    public static class FastEndpointsExtensions
    {
        /// <summary>
        /// Sends created at asynchronously.
        /// </summary>
        /// <typeparam name="TEndpoint"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="ep">The ep.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="response">The response.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns>A Task.</returns>
        public static Task SendCreatedAtAsync<TEndpoint, TResponse>(this IEndpoint ep, object? routeValues, TResponse response, CancellationToken cancellation = default)
            where TEndpoint : IEndpoint
            where TResponse : IErrorOr<object>
        {
            if (!response.IsError)
            {
                return ep.HttpContext.Response.SendCreatedAtAsync<TEndpoint>(routeValues, response.Value, cancellation: cancellation);
            }

            return HandleErrorOr(ep, response, cancellation);
        }

        /// <summary>
        /// Send asynchronously.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="ep">The ep.</param>
        /// <param name="response">The response.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns>A Task.</returns>
        public static Task SendAsync<TResponse>(this IEndpoint ep, TResponse response, CancellationToken cancellation = default)
            where TResponse : IErrorOr<object>
        {
            if (!response.IsError)
            {
                return ep.HttpContext.Response.SendAsync(response.Value, cancellation: cancellation);
            }

            return HandleErrorOr(ep, response, cancellation);
        }

        /// <summary>
        /// Send no content response asynchronously.
        /// </summary>
        /// <typeparam name="TResponse"/>
        /// <param name="ep">The ep.</param>
        /// <param name="response">The response.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns>A Task.</returns>
        public static Task SendNoContentResponseAsync<TResponse>(this IEndpoint ep, TResponse response, CancellationToken cancellation = default)
            where TResponse : IErrorOr
        {
            if (!response.IsError)
            {
                return ep.HttpContext.Response.SendNoContentAsync(cancellation: cancellation);
            }

            return HandleErrorOr(ep, response, cancellation);
        }

        /// <summary>
        /// Handle error or.
        /// </summary>
        /// <typeparam name="TResponse"/>
        /// <param name="ep">The ep.</param>
        /// <param name="response">The response.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns>A Task.</returns>
        /// <exception cref="InvalidOperationException">Invalid operation exception.</exception>
        private static Task HandleErrorOr<TResponse>(IEndpoint ep, TResponse response, CancellationToken cancellation = default)
            where TResponse : IErrorOr
        {
            if (response.Errors?.TrueForAll(error => error.Type == ErrorType.Validation) is true)
            {
                return ep.HttpContext.Response.SendErrorsAsync(
                    failures: [.. response.Errors.Select(error => new ValidationFailure(error.Code, error.Description))],
                    cancellation: cancellation);
            }

            var problem = response.Errors?.Find(error => error.Type != ErrorType.Validation);

            switch (problem?.Type)
            {
                case ErrorType.Conflict:
                    return ep.HttpContext.Response.SendAsync("Duplicate submission!", 409, cancellation: cancellation);
                case ErrorType.NotFound:
                    return ep.HttpContext.Response.SendErrorsAsync(failures: [new ValidationFailure(problem.Value.Code.ToLowerInvariant(), problem.Value.Description)], 404, cancellation: cancellation);
                case ErrorType.Unauthorized:
                    return ep.HttpContext.Response.SendUnauthorizedAsync(cancellation);
                case ErrorType.Forbidden:
                    return ep.HttpContext.Response.SendForbiddenAsync(cancellation);
                case null:
                    throw new InvalidOperationException("No matching endpoint");
            }

            throw new InvalidOperationException("No matching endpoint");
        }
    }
}
