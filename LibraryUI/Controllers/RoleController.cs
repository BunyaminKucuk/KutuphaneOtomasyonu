using DataAccess.Abstract;
using Entity.Identity;
using Library.API.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LibraryUI.Controllers
{
    //[Authorize(Policy = "AdminPolicy")]
    public class RoleController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> RoleList()
        {
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Role/GetAllRoles");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ApplicationRole>>(jsonString);
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            return View(new ApplicationRole());
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(ApplicationRole model)
        {

            //var token = HttpContext.User.FindFirst(ClaimTypes.Authentication).Value;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Role/AddNewRole"), model);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("RoleList", "Role");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddRoleToUser()
        {
            List<SelectListItem> roleValues = (from x in _roleManager.Roles.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = x.RoleName,
                                                   Value = x.Id
                                               }).ToList();
            ViewBag.cv = roleValues;
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserModel model, int id)
        {
            model.User.Id = id;
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/User/AddRoleToUser?User.Id=" + id + "&Role.Id=" + model.Role.Id), model);


            if (responseMessage.IsSuccessStatusCode)
            {
                //return RedirectToAction("Index", "Role");
                return LocalRedirect("/Admin/Dashboard/Index");
            }
            return View();
        }
    }
}
