using System.Net;

namespace TeckShop.Core.Exceptions
{
    /// <summary>
    /// The configuration missing exception.
    /// </summary>
    public class ConfigurationMissingException : CustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
        /// </summary>
        /// <param name="sectionName"></param>
        public ConfigurationMissingException(string sectionName) : base($"{sectionName} Missing in Configurations", HttpStatusCode.NotFound)
        {
        }
    }
}
