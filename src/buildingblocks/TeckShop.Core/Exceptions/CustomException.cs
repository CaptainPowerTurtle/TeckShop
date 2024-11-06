using System.Net;

namespace TeckShop.Core.Exceptions
{
    /// <summary>
    /// The custom exception.
    /// </summary>
    public class CustomException : Exception
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="statusCode">The status code.</param>
        public CustomException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
