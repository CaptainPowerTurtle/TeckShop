using System.ComponentModel.DataAnnotations;
using TeckShop.Core.Options;

namespace TeckShop.Infrastructure.Options
{
    /// <summary>
    /// The app options.
    /// </summary>
    public class AppOptions : IOptionsRoot
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = "TeckShop.WebAPI";
    }
}
