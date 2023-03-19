using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.EntityFramework
{
    public class EfUserRepository : GenericRepository<User>, IUserRepository
    {
        public EfUserRepository(DbContext dbContext) : base(dbContext)
        {

        }
        public LibraryContext? _context { get { return Context as LibraryContext; } }


        public void AddIdentiy(UserManager<ApplicationUser> userManager, User item)
        {
            string username = (item.UserName).Replace(" ", "").Replace("#", "").Replace("ö", "o").Replace("ü", "u");
            

            var user = userManager.FindByNameAsync(username).GetAwaiter().GetResult();

            if (user == null)
            {
                var appuser = new ApplicationUser
                {
                    UserName = username,
                    Email = item.Email,
                    IdentityId = item.IdentityId,
                    Name = item.Name
                };

                if (item.Password == "")
                {
                    item.Password = "1234!";
                }
                IdentityResult result = userManager.CreateAsync(appuser, item.Password).GetAwaiter().GetResult();
                if (result.Succeeded)
                {

                }
                else
                {
                    throw new Exception("Identity hesabı oluşurken hata oluştu ");
                }
            }
            else
            {
                if (user.IdentityId != item.IdentityId)
                {
                    item.IdentityId = user.IdentityId;

                }
            }
        }

        public string UpdateIdentiy(UserManager<ApplicationUser> userManager, User item, List<ApplicationRole> roller)
        {
            var user = userManager.FindByNameAsync(item.UserName).GetAwaiter().GetResult();

            var roles = userManager.GetRolesAsync(user).GetAwaiter().GetResult();
            var result = userManager.RemoveFromRolesAsync(user, roles).GetAwaiter().GetResult();
            if (roller.Count() > 0)
            {
                result = userManager.AddToRolesAsync(user, roller.Select(y => y.Name)).GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    return "Seçilen roller kullanıcıya eklenemiyor.";
                }

            }
            if (user != null)
            {
                user.Email = item.Email;
                user.Name = item.Name;

                result = userManager.UpdateAsync(user).GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    return "Identity kullanıcısı güncellenirken hata oluştu";
                }
            }
            else
            {
                return "Güncellenmek istenilen Identity kullanıcısı bulunamadı";
            }
            return "";
        }

        public ApplicationUser GetIdentityUser(UserManager<ApplicationUser> userManager, string KullaniciAdi)
        {
            var user = userManager.FindByNameAsync(KullaniciAdi).GetAwaiter().GetResult();
            return user;
        }

        public void AddUser(User request)
        {
            User? staff = _context.Users.Where(x => x.UserName == request.UserName && x.IdentityId == request.IdentityId)?.FirstOrDefault();
            bool userNameCheck = _context.Users.Where(x => x.UserName == request.UserName).Any();
            bool epostaCheck = _context.Users.Where(x => x.Email == request.Email).Any();
            if (epostaCheck)
            {
                throw new Exception("Sistemde bu e-posta kayıtlıdır.");
            }
            if (userNameCheck)
            {
                throw new Exception("Sistemde bu kullanıcı adı kayıtlıdır.");
            }
            if (staff != null)
            {
                throw new Exception($"{request.IdentityId} identityid ve {request.UserName} oturumadı adı ile kullanıcı bulunmaktadır.");
            }

            _context.Users.Add(request);
            _context.SaveChanges();


        }
    }
}

