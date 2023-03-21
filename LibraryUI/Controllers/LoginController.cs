using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace LibraryUI.Controllers
{
 
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Login/Login"), model);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadFromJsonAsync<List<string>>();
                if (responseContent != null && responseContent.Count >= 3)
                {
                    var jwt = responseContent[0];
                    var identityId = responseContent[1];
                    var userCheck = responseContent[2];

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Authentication,jwt),
                        new Claim(ClaimTypes.NameIdentifier, identityId),
                        new Claim("UserCheck", userCheck),
                    };
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    var decodedJwt = jwtSecurityTokenHandler.ReadJwtToken(jwt);

                    foreach (var claim in decodedJwt.Claims)
                    {
                        if (claim.Type != JwtRegisteredClaimNames.Sub)
                        {
                            claims.Add(claim);
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    var responseRole = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Role/GetUserRoleAndIsActive"), model);
                    var jsonStringRole = await responseRole.Content.ReadAsStringAsync();
                    dynamic response = JsonConvert.DeserializeObject(jsonStringRole);

                    if (response.isActive == true)
                    {
                        switch (response.roles.ToString())
                        {
                            case "Admin":
                                return RedirectToAction("UserList", "Admin");
                            case "Librarian":
                                return RedirectToAction("BookList", "Book");
                            case "Customer":
                                return RedirectToAction("UserBookList", "Customer");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Hesabınız henüz aktive edilmemiştir.Daha sonra tekrar deneyiz..!";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Hatalı yanıt alındı.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "İstek gönderilirken bir hata oluştu.";
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Login/Register"), model);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
