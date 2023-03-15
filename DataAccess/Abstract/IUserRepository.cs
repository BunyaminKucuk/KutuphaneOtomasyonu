using Entity.Concrete;
using Entity.Identity;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Abstract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        void AddIdentiy(UserManager<ApplicationUser> userManager, User item);
        string UpdateIdentiy(UserManager<ApplicationUser> userManager, User item, List<ApplicationRole> roller);
        ApplicationUser GetIdentityUser(UserManager<ApplicationUser> userManager, string KullaniciAdi);
        void AddUser(User request);
    }
}
