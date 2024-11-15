using System.Net;

namespace TeckShop.Core.Exceptions
{
    /// <summary>
    /// The invalid transaction exception.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="InvalidTransactionException"/> class.
    /// </remarks>
    /// <param name="message">The message.</param>
    public class InvalidTransactionException(string message) : CustomException(message, HttpStatusCode.InternalServerError)
    {
    }
}
