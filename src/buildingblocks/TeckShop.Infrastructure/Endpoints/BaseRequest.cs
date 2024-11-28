using FastEndpoints;

namespace TeckShop.Infrastructure.Endpoints
{
    /// <summary>
    /// The base request.
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// Gets or sets the tenant.
        /// </summary>
        [FromHeader("x-tenant-id")]
        public required string Tenant { get; set; }
    }
}
