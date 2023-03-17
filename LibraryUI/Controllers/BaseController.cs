using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace LibraryUI.Controllers
{
    public class BaseController : Controller
    {
        //private readonly HttpClient _httpClient;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            ViewBag.Role = role;
            base.OnActionExecuting(context);
        }
    }
}
