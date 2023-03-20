using DataAccess.Abstract;
using Entity.Concrete;
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
    public class UserController : Controller
    {
        #region Fields

        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        #endregion


        #region Ctor

        public UserController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        #endregion


        #region Methods

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _unitOfWork.User.GetListAll().Where(x => x.Deleted == false).ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("AddNewUser")]
        public async void AddnewUser(User model)
        {
            try
            {
                User user = new User();
                bool userCheck = _unitOfWork.User.GetListAll().Where(x => x.UserName == model.UserName).Any();
                bool mailCheck = _unitOfWork.User.GetListAll().Where(x => x.Email == model.Email).Any();

                if (mailCheck || userCheck)
                {
                    throw new Exception("Bu bilgilere sahip kullanıcı vardır.");
                }

                ApplicationUser applicationUser = new ApplicationUser
                {
                    Email = model.Email,
                    IdentityId = Guid.NewGuid(),
                    Name = model.Name,
                    UserName = model.UserName
                };

                user.Name = model.Name;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.IdentityId = Guid.NewGuid();
                user.Deleted = false;
                user.IsActive = false;


                _unitOfWork.User.AddIdentiy(_userManager, user);
                _unitOfWork.User.AddUser(user);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel model)
        {
            try
            {

                var checkUser = _unitOfWork.User.GetListAll().Where(x => x.UserName == model.UserName && x.IdentityId == model.IdentityId).FirstOrDefault();
                if (checkUser == null)
                {
                    throw new Exception("Belirtilen kullanıcı bulunamadı.");
                }
                bool mailCheck = _unitOfWork.User.GetListAll().Where(x => x.Email == model.Email).Any();
                if (mailCheck && checkUser.Email != model.Email)
                {
                    throw new Exception("Belirtilen mail adresi sitemde mevcuttur.Lütfen yeni bir mail adresi giriniz.");
                }

                var identityCheck = _userManager.Users.Where(x => x.IdentityId == checkUser.IdentityId).FirstOrDefault();
                if (identityCheck == null)
                {
                    throw new Exception("Identity kullanıcısı bulunamadı.");
                }

                var user = await _userManager.FindByNameAsync(model.UserName);
                var roles = await _userManager.GetRolesAsync(user);
                var applicationRoles = new List<ApplicationRole>();
                foreach (var roleName in roles)
                {
                    var applicationRole = await _roleManager.FindByNameAsync(roleName);
                    applicationRoles.Add(applicationRole);
                }

                checkUser.Name = model.Name;
                checkUser.UserName = model.UserName;
                checkUser.Email = model.Email;
                checkUser.PhoneNumber = model.PhoneNumber;
                checkUser.UserName = model.UserName;
                checkUser.Password = model.Password;
                checkUser.IsActive = model.IsActive;
                checkUser.LibraryStatus = model.LibraryStatus;
                checkUser.Deleted = model.Deleted;



                _unitOfWork.User.UpdateIdentiy(_userManager, checkUser, applicationRoles);
                _unitOfWork.User.Update(checkUser);
                _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return BadRequest();
        }

        [HttpPost("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser([FromBody]AddRoleToUserModel model)
        {
            try
            {
                var userCheck = _unitOfWork.User.GetListAll().Where(x => x.Id == model.User.Id).FirstOrDefault();
                if (userCheck == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }
                if (Guid.Empty == userCheck.IdentityId)
                {
                    throw new Exception("Identity bilgisi alınamadı.");
                }
                var identityUserCheck = _userManager.Users.Where(x => x.IdentityId == userCheck.IdentityId).FirstOrDefault();
                if (identityUserCheck == null)
                {
                    throw new Exception("Identity kullanıcısı bulunamadı");
                }

                List<ApplicationRole> roleList = new List<ApplicationRole>();

                var rol = await _roleManager.FindByIdAsync(model.Role.Id);
                if (!string.IsNullOrEmpty(rol.RoleName)) roleList.Add(rol);


                _unitOfWork.User.UpdateIdentiy(_userManager, userCheck, roleList);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return BadRequest();
        }

        [HttpPost("UserDelete")]
        public async void UserDelete([FromBody] int userId)
        {
            try
            {
                var checkUser = _unitOfWork.User.GetListAll().Where(x => x.Id == userId).FirstOrDefault();

                if (checkUser == null)
                {
                    throw new Exception("Belirtilen kullanıcı bulunamadı.");
                }

                checkUser.Name = checkUser.Name;
                checkUser.UserName = checkUser.UserName;
                checkUser.Email = checkUser.Email;
                checkUser.PhoneNumber = checkUser.PhoneNumber;
                checkUser.UserName = checkUser.UserName;
                checkUser.Password = checkUser.Password;
                checkUser.IsActive = false;
                checkUser.LibraryStatus = checkUser.LibraryStatus;
                checkUser.Id = checkUser.Id;
                checkUser.IdentityId = checkUser.IdentityId;
                checkUser.Deleted = true;

                _unitOfWork.User.Update(checkUser);
                _unitOfWork.SaveChanges();


            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetUsersByRole")]
        public List<User> GetUserByRole(string roleName)
        {
            try
            {
                if (string.IsNullOrEmpty(roleName))
                {
                    throw new Exception("Rol boş bırakılamaz.");
                }
                var role = _roleManager.FindByNameAsync(roleName).Result;
                if (role == null)
                {
                    throw new Exception("Rol bulunamadı.");
                }
                var identityUsers = _userManager.GetUsersInRoleAsync(roleName).Result;
                List<User> users = new List<User>();
                if (identityUsers.Count() > 0)
                {
                    foreach (var item in identityUsers)
                    {
                        var user = _unitOfWork.User.GetListAll().Where(x => x.IdentityId == item.IdentityId).FirstOrDefault();
                        if (user != null) users.Add(user);
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet("UserGetById")]
        public UserModel UserGetById(int id)
        {
            try
            {
                var userCheck = _unitOfWork.User.GetListAll().FirstOrDefault(x => x.Id == id);
                if (userCheck == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                if (Guid.Empty == userCheck.IdentityId)
                {
                    throw new Exception("Identity bilgisi alınamadı.");
                }

                var identityUserCheck = _userManager.Users.FirstOrDefault(x => x.IdentityId == userCheck.IdentityId);
                if (identityUserCheck == null)
                {
                    throw new Exception("Identity kullanıcısı bulunamadı");
                }

                UserModel userModel = new UserModel
                {
                    Name = userCheck.Name,
                    UserName = userCheck.UserName,
                    Email = userCheck.Email,
                    IsActive = userCheck.IsActive,
                    IdentityId = userCheck.IdentityId,
                    Deleted = userCheck.Deleted,
                    Id = userCheck.Id,
                    PhoneNumber = userCheck.PhoneNumber
                };

                return userModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //[HttpGet("Search")]
        //public async Task<IActionResult> Search(string query)
        //{
        //    var users = _unitOfWork.User.GetListAll().Where(u => u.Name.Contains(query) || u.Email.Contains(query) || u.UserName.Contains(query)).ToList();
        //    return Ok(users);
        //}

        #endregion

    }
}

