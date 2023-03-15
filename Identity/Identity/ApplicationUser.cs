using Microsoft.AspNetCore.Identity;

namespace Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Guid IdentityId { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public bool IsActive { get; set; }
    }
}
