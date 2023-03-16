using System.IdentityModel.Tokens.Jwt;
using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Azure;
using DataAccess.Abstract;

namespace LibraryUI.Controllers
{
    //[Authorize(Policy = "Admin")]
    public class LoginController : Controller
    {
        private readonly SignInManager<LoginModel> _signInManager;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }


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

                var user = await _userManager.FindByNameAsync(model.UserName);
                var userRole = await _userManager.GetRolesAsync(user);
                switch (userRole.FirstOrDefault())
                {
                    case "Admin":
                        return RedirectToAction("Index", "Admin");
                        break;
                    case "Librarian":
                        return RedirectToAction("Index", "Admin");
                        break;
                    case "Customer":
                        return RedirectToAction("TakeOnBook", "Book");
                        break;
                }
            }
            else
            {
                return RedirectToAction("LogOut", "Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register(RegisterModel model)
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}
