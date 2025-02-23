using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.Domain.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _NET_MinimalAPI.Presentation.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdministratorController : Controller
    {
        private readonly IAdministratorService _administratorService;
        private readonly IConfiguration _configuration;

        public AdministratorController(IAdministratorService administratorService, IConfiguration configuration)
        {
            this._administratorService = administratorService;
            this._configuration = configuration;
        }

        [Route("/login")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDTO loginDTO, IAdministratorService administratorService)
        {

            var adm = administratorService.Login(loginDTO);
            if (adm != null)
            {
                var _key = _configuration.GetSection("Jwt").Value;

                string _tk = CreateJwtToken(adm, _key!);
                return Ok(new AdministratorMVTk
                {
                    Id = adm.Id,
                    Mail = adm.Mail,
                    Profile = adm.Profile,
                    Token = _tk
                });
            }
            else
            {
                return Unauthorized();
            }
        }


        private string CreateJwtToken(Administrator administrator, string key)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("Mail", administrator.Mail),
                new Claim("Profile", administrator.Profile),
                new Claim(ClaimTypes.Role, administrator.Profile)
            };


            var tk = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tk);
        }
    }
}
