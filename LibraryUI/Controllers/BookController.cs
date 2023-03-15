using Entity.Concrete;
using Library.API.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LibraryUI.Controllers
{
    //[Authorize(Roles = "AdminPolicy")]
    public class BookController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            //var response=_httpClient.GetAsync()
        }
        public async Task<IActionResult> Index()
        {
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Book/GetBookList");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Book>>(jsonString);
            return View(values);
        }
        public async Task<IActionResult> BookReadMore(int id)
        {
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Book/GetBookById?bookId=" + id);
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Book>>(jsonString);
            return View(values);
        }


        [HttpGet]
        public async Task<IActionResult> TakeOnBook()
        {
            return View(new TakeOnBookModel());
        }

        [HttpPost]
        public async Task<IActionResult> TakeOnBook(int bookId, int userId)
        {

                    //var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            TakeOnBookModel takeOfBook = new TakeOnBookModel();
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/AddBookToUser?bookId=2&userId=5"), takeOfBook);
            var request = _httpContextAccessor.HttpContext.Request;
            if (request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                // Header var
                // authorizationHeader değişkeni header'ın değerini içerir
            }
            else
            {
                // Header yok
            }





            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonString);

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jsonString);
            var claims = jwt.Claims.ToList();

            claims.Add(new Claim(ClaimTypes.Authentication, jsonString));
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            var values = JsonConvert.DeserializeObject<List<Book>>(jsonString);
            return View(values);
        }
    }
}


