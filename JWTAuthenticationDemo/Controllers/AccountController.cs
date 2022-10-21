using JWTAuthenticationDemo.Models;
using JWTAuthenticationDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace JWTAuthenticationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettings jwtSettings;
        public AccountController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }
        private IEnumerable<Users> logins = new List<Users>() {
            new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "adminakp@gmail.com",
                        UserName = "Admin",
                        Password = "Admin",
                },
                new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "adminakp@gmail.com",
                        UserName = "User1",
                        Password = "Admin",
                }
        };
        [HttpPost]
        public IActionResult GetToken(UserLogins userLogins)
        {
            try
            {
                var Token = new UserTokens();
                //var Valid = logins.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                var Valid = logins.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase) 
                                        && x.Password.Equals(userLogins.Password, StringComparison.OrdinalIgnoreCase));
                if (Valid)
                {
                    //var user = logins.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));

                    var user = logins.Where(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase)
                                        && x.Password.Equals(userLogins.Password, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    HttpContext.Session.SetString("username", userLogins.UserName);
                    HttpContext.Session.SetString("id",Convert.ToString(new Guid()));

                    Token = JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        EmailId = user.EmailId,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Id = user.Id,
                    }, jwtSettings);
                }
                else
                {
                    return BadRequest($"wrong password");
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Get List of UserAccounts
        /// </summary>
        /// <returns>List Of UserAccounts</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetList()
        {
            var x = new
            {
                x = HttpContext.Session.GetString("username"),
                y = HttpContext.Session.GetString("id"),
            };

            return Ok(x) ;
        }
    }
}
