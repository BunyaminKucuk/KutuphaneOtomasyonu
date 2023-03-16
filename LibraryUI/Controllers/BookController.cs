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
using Azure.Core;

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
        public async Task<IActionResult> TakeOnBook(TakeOfBook model)
        {
            
            var token = HttpContext.User.FindFirst(ClaimTypes.Authentication).Value;
            TakeOfBook takeOfBook = new TakeOfBook();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/AddBookToUser?bookId=" + model.UserId + "&userId=" + model.BookId), model);


            //var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claims = jwt.Claims.ToList();

            //claims.Add(new Claim(ClaimTypes.Authentication, jsonString));
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            //var values = JsonConvert.DeserializeObject<List<Book>>(jsonString);
            return View();
        }
    }
}


