namespace TeckShop.Core.Auth
{
    /// <summary>
    /// The organization representation.
    /// </summary>
    public class OrganizationRepresentation
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Create a brand.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>A OrganizationRepresentation object.</returns>
        public static OrganizationRepresentation Create(
            Guid id,
            string name)
        {
            return new()
            {
                Id = id,
                Name = name
            };
        }
    }
}
