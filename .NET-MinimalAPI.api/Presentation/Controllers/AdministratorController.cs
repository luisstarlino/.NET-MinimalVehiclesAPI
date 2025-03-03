using _NET_MinimalAPI.Domain.DTOs;
using _NET_MinimalAPI.Domain.Enuns;
using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.Domain.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Permissions;
using System.Text;

namespace _NET_MinimalAPI.Presentation.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdministratorController : Controller
    {
        private readonly IAdministratorService _administratorService;
        private readonly IConfiguration _configuration;

        public AdministratorController(IAdministratorService administratorService, IConfiguration configuration)
        {
            this._administratorService = administratorService;
            this._configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <param name="administratorService"></param>
        /// <returns></returns>
        [Route("/login")]
        [HttpPost]
        [Tags("Admin")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {

            var adm = _administratorService.Login(loginDTO);
            if (adm != null)
            {
                var _key = GetConfigurationTokenKey();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="admDTO"></param>
        /// <param name="administratorService"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("Admin")]
        [Authorize(Roles ="adm")]
        public IActionResult CreateAdministrator([FromBody] AdministratorDTO admDTO)
        {
            // --- Handle
            var messagesError = new ErrorHandler();
            if (admDTO.Profile == null) messagesError.Messages.Add("Parameter 'Profile' as required!");
            if (String.IsNullOrEmpty(admDTO.Password)) messagesError.Messages.Add("Parameter 'Password' as required!");
            if (String.IsNullOrEmpty(admDTO.Mail)) messagesError.Messages.Add("Parameter 'Mail' as required!");


            if (messagesError.Messages.Count > 0) return BadRequest(messagesError);

            var adm = new Administrator
            {
                Mail = admDTO.Mail,
                Password = admDTO.Password,
                Profile = admDTO.Profile.ToString() ?? ProfileENUM.editor.ToString(),

            };

            _administratorService.Add(adm);

            return Created($"/vehicle/{adm}", adm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="administratorService"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("Admin")]
        [Authorize(Roles = "adm")]
        public IActionResult GetAllAdmistrators([FromQuery] int? page)
        {
            var admsDB = _administratorService.GetAll(page);
            var admsMV = new List<AdministratorMV>();

            if (admsDB.Count == 0) return NoContent();

            foreach (Administrator adm in admsDB)
            {
                admsMV.Add(new AdministratorMV()
                {
                    Id = adm.Id,
                    Mail = adm.Mail,
                    Profile = adm.Profile,
                });
            };

            return Ok(admsMV);
        }

        [HttpGet]
        [Tags("Admin")]
        [Route("{id}")]
        [Authorize(Roles = "adm")]
        public IActionResult GetUniqueAdministrator([FromRoute] int id)
        {
            var admDB = _administratorService.GetUniqueById(id);

            if (admDB == null) return NotFound();


            else return Ok(new AdministratorMV()
            {
                Id = admDB.Id,
                Profile = admDB.Profile,
                Mail = admDB.Mail,
            });
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

        private string GetConfigurationTokenKey()
        {
            var foundValue = _configuration["Jwt:Key"];

            if(string.IsNullOrEmpty(foundValue))
            {
                return "";
            }

            return foundValue;
        }
    }
}
