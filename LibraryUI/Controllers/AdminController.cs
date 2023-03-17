using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LibraryUI.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : BaseController
    {
        private readonly HttpClient _httpClient = new HttpClient();

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
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/User/GetAllUsers");


            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<User>>(jsonString);
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> UserEdit(int id)
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/User/UserGetById?id=" + id);

            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserModel>(jsonString);
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserModel model)
        {

            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/User/UpdateUser", model);

            if (responseMessage.IsSuccessStatusCode)
            {
                RedirectToAction("UserList", "Admin");
            }

            return View();
            //var userCheck = _unitOfWork.User.GetListAll().Where(x => x.Id == model.Id).FirstOrDefault();
            //return View();
        }
    }
}
