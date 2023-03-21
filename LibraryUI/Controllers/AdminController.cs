using Entity.Concrete;
using Library.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LibraryUI.Controllers
{
    public class AdminController : BaseController
    {
        private readonly HttpClient _httpClient = new HttpClient();

        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult AdminNavbarPartial()
        {
            return PartialView();
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UserList(string query)
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/User/GetAllUsers");

            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<User>>(jsonString);
            return View(values);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> UserEdit(int id)
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/User/UserGetById?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var userModel = JsonConvert.DeserializeObject<UserModel>(jsonString);
                return View(userModel);
            }

            return View();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserModel model)
        {

            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/User/UpdateUser", model);
            return RedirectToAction("UserList", "Admin");

        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> UserDelete(int id)
        {

            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/User/UserDelete"), id);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList", "Admin");
            }

            return Ok();

        }
    }
}
