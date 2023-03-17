using DataAccess.Abstract;
using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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

        [HttpPost("GetUserRoleAndIsActive")]
        public IActionResult GetUserRole([FromBody]LoginModel model)
        {
            List<ApplicationRole> response = new List<ApplicationRole>();
            try
            {
                var userCheck = _unitOfWork.User.GetListAll().Where(x => x.UserName == model.UserName).FirstOrDefault();
                var appUser = _userManager.Users.Where(x => x.IdentityId == userCheck.IdentityId).FirstOrDefault();
                var userIsActive = userCheck.IsActive;

                if (appUser == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                if (Guid.Empty == appUser.IdentityId)
                {
                    throw new Exception("Identity bilgisi alınamadı.");
                }

                var userRoles = _userManager.GetRolesAsync(appUser).Result.FirstOrDefault();

                var result = new { Roles = userRoles, IsActive = userIsActive };
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
