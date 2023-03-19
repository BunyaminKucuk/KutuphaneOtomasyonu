using Microsoft.AspNetCore.Identity;

namespace Entity.Identity
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the user GUID
        /// </summary>
        public Guid IdentityId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the discount is active
        /// </summary>
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}
