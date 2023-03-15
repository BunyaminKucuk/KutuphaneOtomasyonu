using Microsoft.AspNetCore.Identity;

namespace Identity
{
    public class ApplicationRole : IdentityRole
    {
        public string RoleName { get; set; }
        public DateTime CreatedTime { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
