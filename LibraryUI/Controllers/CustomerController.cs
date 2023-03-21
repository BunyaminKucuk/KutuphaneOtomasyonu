using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Entity.Concrete;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Net.Http;
using NuGet.Protocol;

namespace LibraryUI.Controllers
{
    [Authorize(Policy = "CustomerPolicy")]
    public class CustomerController : BaseController
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<IActionResult> UserBookList()
        {
            var userCheck = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserCheck")?.Value;
            var userId = Convert.ToInt32(userCheck);

            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Book/GetUserBooks?userId=" + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);
                return View(books);
            }
            return View();
        }


        public async Task<IActionResult> Index()
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Book/AssignableBookList");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Book>>(jsonString);
            return View(values);
        }

        public async Task<IActionResult> DeleteUserBook(int id)
        {
            var userCheck = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserCheck")?.Value;
            var userId = Convert.ToInt32(userCheck);
            TakeOfBook model = new TakeOfBook();
            model.BookId = id;
            model.UserId = userId;

            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/DeleteUserBook"), model);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("UserBookList", "Customer");
            }
            return View();
        }

        public async Task<IActionResult> DeleteRequestBook(int id)
        {
            var userCheck = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserCheck")?.Value;
            var userId = Convert.ToInt32(userCheck);
            TakeOfBook model = new TakeOfBook();
            model.UserId = userId;
            model.BookId = id;

            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/DeleteRequestBook"), model);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("TakeBookList", "Customer");
            }
            return View();
        }

        public async Task<IActionResult> TakeBook(int id)
        {
            TakeOfBook model = new TakeOfBook();
            var userCheck = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserCheck")?.Value;
            var userId = Convert.ToInt32(userCheck);
            model.UserId = userId;
            model.BookId = id;
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/TakeBook"), model);

            return RedirectToAction("TakeBookList", "Customer");

        }

        public async Task<IActionResult> TakeBookList()
        {
            var userCheck = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserCheck")?.Value;
            var userId = Convert.ToInt32(userCheck);

            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Book/DesiredBooks?userId=" + userId);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);
                return View(books);

            }

            return View();
        }

    }
}

