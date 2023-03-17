using Library.API.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace LibraryUI.Controllers
{
    //[Authorize(Policy = "Admin")]
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
            if (ModelState.IsValid)
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Login/Login"), model);
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jsonString);

                var claims = jwt.Claims.ToList();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.RawData);

                claims.Add(new Claim(ClaimTypes.Authentication, jsonString));
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
                            return RedirectToAction("Index", "Admin");
                            break;
                        case "Librarian":
                            return RedirectToAction("TakeOnBook", "Book");
                            break;
                        case "Customer":
                            return RedirectToAction("TakeOnBook", "Book");
                            break;
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Hesabınız henüz aktive edilmemiştir.Daha sonra tekrar deneyiz..!";
                }
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

        //[]
        //public async Task<IActionResult> LogOut()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Index", "Login");
        //}
    }
}
