using DataAccess.Abstract;
using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("GetAllRoles")]
        public List<ApplicationRole> GetAllRoles()
        {
            try
            {
                return _roleManager.Roles.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetUserRole")]
        public List<string> GetUserRole(Guid identityId)
        {
            List<ApplicationRole> response = new List<ApplicationRole>();
            try
            {

                var appUser = _userManager.Users.Where(x => x.IdentityId == identityId).FirstOrDefault();

                if (appUser == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                if (Guid.Empty == appUser.IdentityId)
                {
                    throw new Exception("Identity bilgisi alınamadı.");
                }
                return _userManager.GetRolesAsync(appUser).Result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("AddNewRole")]
        public void AddNewRole([FromBody] RoleModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    throw new Exception("Rol boş bırakılamaz.");
                }
                ApplicationRole ar = new ApplicationRole
                {
                    Name = model.Name,
                    RoleName = model.Name,
                    CreatedTime = System.DateTime.Now,
                };
                var result = _roleManager.CreateAsync(ar).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
