using System.Net;

namespace TeckShop.Core.Exceptions
{
    /// <summary>
    /// The configuration missing exception.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
    /// </remarks>
    /// <param name="sectionName"></param>
    public class ConfigurationMissingException(string sectionName) : CustomException($"{sectionName} Missing in Configurations", HttpStatusCode.NotFound)
    {
    }
}
