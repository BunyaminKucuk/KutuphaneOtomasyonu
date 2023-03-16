using Entity.Concrete;
using Entity.Identity;

namespace Library.API.Controllers
{
    public class AddRoleToUserModel
    {
        public virtual User User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
