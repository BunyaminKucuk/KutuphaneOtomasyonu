using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LibraryUI.Controllers
{
    //[Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult AdminNavbarPartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> UserList()
        {

            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/User/GetAllUsers");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<User>>(jsonString);
            return View(values);

        }

        [HttpGet]
        public async Task<IActionResult> UserEdit(int id)
        {
            var userCheck = _unitOfWork.User.GetListAll().Where(x => x.Id == id).FirstOrDefault();
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

            UserModel userModel = new UserModel();
            //var userCheck = _userManager.GetUserName(userName);
            userModel.Name = userCheck.Name;
            userModel.UserName = userCheck.UserName;
            userModel.Email = userCheck.Email;
            userModel.IsActive = userCheck.IsActive;
            userModel.IdentityId = userCheck.IdentityId;
            userModel.Deleted = userCheck.Deleted;
            userModel.Id = userCheck.Id;
            userModel.PhoneNumber = userCheck.PhoneNumber;

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserModel model)
        {

            var responseMessage = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/User/UpdateUser",model);

            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<User>>(jsonString);
            return View(values);
            //var userCheck = _unitOfWork.User.GetListAll().Where(x => x.Id == model.Id).FirstOrDefault();
            //return View();
        }
    }
}
