using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    // [ApiController]
    public class UserController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;


        public UserController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllUsers")]
        public List<ApplicationUser> GetAllUsers()
        {
            try
            {
                return _userManager.Users.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
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
                    IsActive = "true",
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

        //user id gelcek
        [HttpPatch("UpdateUser")]
        public async void UpdateUser([FromBody] UserModel model, string userId = "2fa4b006-b22f-4e94-b379-2ae505db740e")
        {
            try
            {
                var checkUser = _unitOfWork.User.GetListAll().Where(x => x.UserName == model.UserName && x.IdentityId == model.IdentityId).FirstOrDefault();
                List<ApplicationRole> role = new List<ApplicationRole>();

                if (checkUser == null)
                {
                    throw new Exception("Belirtilen kullanıcı bulunamadı.");
                }
                bool mailCheck = _unitOfWork.User.GetListAll().Where(x => x.Email == model.Email).Any();
                if (mailCheck && checkUser.Email != model.Email)
                {
                    throw new Exception("Belirtilen mail adresi sitemde mevcuttur.Lütfen yeni bir mail adresi giriniz.");
                }
                bool userNameCheck = _unitOfWork.User.GetListAll().Where(x => x.UserName == model.UserName).Any();
                if (userNameCheck)
                {
                    throw new Exception("Belirtilen kullanıcı adı sitemde mevcuttur.Lütfen yeni bir kullanıcı adı giriniz.");
                }
                var identityCheck = _userManager.Users.Where(x => x.IdentityId == checkUser.IdentityId).FirstOrDefault();
                if (identityCheck == null)
                {
                    throw new Exception("Identity kullanıcısı bulunamadı.");
                }

                checkUser.Name = model.Name;
                checkUser.UserName = model.UserName;
                checkUser.Email = model.Email;
                checkUser.PhoneNumber = model.PhoneNumber;
                checkUser.UserName = model.UserName;
                checkUser.IdentityId = model.IdentityId;
                checkUser.Password = model.Password;
                checkUser.IsActive = model.IsActive;
                checkUser.LibraryStatus = model.LibraryStatus;
                checkUser.Deleted = model.Deleted;

                _unitOfWork.User.UpdateIdentiy(_userManager, checkUser, role);
                _unitOfWork.User.Update(checkUser);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("AddRoleToUser")]
        public async void AddRoleToUser(List<string> roles, int userId)
        {
            try
            {
                var userCheck = _unitOfWork.User.GetListAll().Where(x => x.Id == userId).FirstOrDefault();
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
                foreach (var item in roles)
                {
                    var rol = await _roleManager.FindByNameAsync(item);
                    if (!string.IsNullOrEmpty(rol.RoleName)) roleList.Add(rol);
                }

                _unitOfWork.User.UpdateIdentiy(_userManager, userCheck, roleList);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPatch("DeleteUser")]
        public async void DeleteUser(string userId)
        {
            try
            {
                var checkUser = _userManager.Users.ToList().Where(x => x.Id == userId).FirstOrDefault();
                if (checkUser == null)
                {
                    throw new Exception("Belirtilen kullanıcı bulunamadı.");
                }
                checkUser.Deleted = true;
                var result = _userManager.UpdateAsync(checkUser).GetAwaiter().GetResult();

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetActiveUsers")]
        public List<User> GetActiveStaffs()
        {
            try
            {
                return _unitOfWork.User.GetListAll().Where(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetUsersByRole")]
        public List<User> GetStaffsByRole(string roleName)
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
    }
}

