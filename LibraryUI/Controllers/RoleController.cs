using Entity.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Library.API.Model;

namespace LibraryUI.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class RoleController : BaseController
    {
        private readonly HttpClient _httpClient = new HttpClient();


        public async Task<IActionResult> RoleList()
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Role/GetAllRoles");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ApplicationRole>>(jsonString);
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            return View(new ApplicationRole());
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(ApplicationRole model)
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Role/AddNewRole"), model);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("RoleList", "Role");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddRoleToUser()
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Role/GetAllRoles");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ApplicationRole>>(jsonString);
            ViewBag.cv = new SelectList(values, "Id", "Name");
            return View(new AddRoleToUserModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserModel model, int id)
        {
            model.User.Id = id;

            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/User/AddRoleToUser"), model);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList", "Admin");
            }
            return View();
        }
    }
}
