﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Library.API.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DataAccess.Abstract;
using Entity.Concrete;
using Microsoft.IdentityModel.Tokens;
using Entity.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        #region Fields

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public LoginController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods

        [HttpPost("Login")]
        public async Task<string> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                //vs kapat tekrar aç tekrar eder
                var authClaims = new List<Claim>
                {
                    //ikidir continue yerine stack fire diye bişeye basıyon amk dikkat et abi f11 basıyorum olamyaınca nbasıyorum
                    new Claim(ClaimTypes.Name, user.UserName),
                    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);
                var A = new JwtSecurityTokenHandler().WriteToken(token);


                return (A);
            }

            return "gg";
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new()
            {
                Email = model.Email,
                IdentityId = Guid.NewGuid(),
                UserName = model.UserName,
                IsActive = "0"
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            User checkUser = new User()
            {
                IdentityId = user.IdentityId,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = false,
                Deleted = false,
                Password = model.Password
            };

            _unitOfWork.User.Insert(checkUser);
            _unitOfWork.SaveChanges();

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Audience"],
                expires: DateTime.Now.AddMinutes(120),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        #endregion

    }
}
