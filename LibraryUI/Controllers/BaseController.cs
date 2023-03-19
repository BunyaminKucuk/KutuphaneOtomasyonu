using Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using Entity.Identity;

namespace LibraryUI.Controllers
{
    public class BaseController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userName = HttpContext.User.Identity.Name;
            ViewBag.Role = role;
            ViewBag.UserName = userName;
            base.OnActionExecuting(context);
        }
    }
}
