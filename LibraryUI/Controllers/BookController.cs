using Entity.Concrete;
using Library.API.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LibraryUI.Controllers
{
    //[Authorize(Roles = "LibraryPolicy,")]
    public class BookController : BaseController
    {
        private readonly HttpClient _httpClient = new HttpClient();

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
        public async Task<IActionResult> TakeOnBook(TakeOfBook model)
        {

            var token = HttpContext.User.FindFirst(ClaimTypes.Authentication).Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/AddBookToUser" ), model);


            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claims = jwt.Claims.ToList();

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return View();
        }
    }
}


