using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentManagmentAPI.Model;


namespace StudentManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _SignManager;
        private IConfiguration _config; 
        public byte[] Encoding { get; private set; }

        public ApplicationUserController(UserManager<ApplicationUser> _u, SignInManager<ApplicationUser> _s , IConfiguration _i)
        {
            _userManager = _u;
            _SignManager = _s;
            _config = _i;
        }

        [HttpPost]
        [Route("Register")]
        //post :  /api/ApplicationUser/Register
        public async Task<object> StudentResgistration(ApplicationUserModel model)
        {
            var _user = new ApplicationUser()
            {
                UserName = model.Username,
                FullName = model.FullName,
                Email = model.Email
            };
            try
            {
                var result = await _userManager.CreateAsync(_user, model.Password);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login (LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var toeknDesciptpr = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString())
                    }),

                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["ApplicationSetting:jwt_secret"])),
                    SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(toeknDesciptpr);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "UserName or password not matching" });
                    
            }
              
    }
}
