using System.Net;

namespace TeckShop.Core.Exceptions
{
    /// <summary>
    /// The invalid operation exception.
    /// </summary>
    public class InvalidOperationException : CustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidOperationException(string message) : base(message, HttpStatusCode.InternalServerError)
        {
        }
    }
}
