using System.Net;

namespace TeckShop.Core.Exceptions
{
    /// <summary>
    /// The invalid transaction exception.
    /// </summary>
    public class InvalidTransactionException : CustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTransactionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidTransactionException(string message) : base(message, HttpStatusCode.InternalServerError)
        {
        }
    }
}
