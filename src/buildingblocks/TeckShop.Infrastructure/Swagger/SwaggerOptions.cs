using TeckShop.Core.Options;

namespace TeckShop.Infrastructure.Swagger
{
    /// <summary>
    /// The swagger options.
    /// </summary>
    public class SwaggerOptions : IOptionsRoot
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string? Email { get; set; }
    }
}
