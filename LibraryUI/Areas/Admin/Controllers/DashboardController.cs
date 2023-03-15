using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace LibraryUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
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

        public async Task<IActionResult> GetUserList()
        {

            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/User/GetAllUsers");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<User>>(jsonString);
            return View(values);

        }
    }
}
