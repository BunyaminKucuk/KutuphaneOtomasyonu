using Microsoft.AspNetCore.Identity;

namespace Entity.Identity
{
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// Gets or sets UserRole
        /// </summary>
        public string RoleName { get; set; }   
        
        /// <summary>
        /// Gets or sets CreatedTime
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets UserRoles
        /// </summary>
        public ICollection<ApplicationUserRole>? UserRoles { get; set; }
    }
}
