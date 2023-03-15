using Entity.Concrete;
using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Identity;

namespace LibraryUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();



        public async Task<IActionResult> Index()
        {
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Role/GetAllRoles");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ApplicationRole>>(jsonString);
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> AddNewRole()
        {
            return View(new ApplicationRole());
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRole(ApplicationRole model)
        {

            //var token = HttpContext.User.FindFirst(ClaimTypes.Authentication).Value;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Role/AddNewRole"), model);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Dashboard");

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddRoleToUser(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(List<string> roles, int userId)
        {
            //var jsonRole = JsonConvert.SerializeObject(model);
            //var stringContent = new StringContent(jsonRole, Encoding.UTF8, "application/json");
            //var responseMessage = await _httpClient.PutAsync("https://localhost:44332/api/Role/AddRoleToUser", stringContent);
            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("Index", "Role");
            //}
            return View();
        }
    }
}
