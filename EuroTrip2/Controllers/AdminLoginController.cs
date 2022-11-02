using EuroTrip2.Contexts;
using EuroTrip2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EuroTrip2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public readonly FlightDBContext _context;
        public AdminLoginController(IConfiguration configuration, FlightDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        //[HttpPost]
        //public IActionResult Login([FromBody] Admin user)
        //{
        //    var userAvailable = _context.Admins.Where(u => u.Email == user.Email && u.AdminPassword == user.AdminPassword).FirstOrDefault();
        //    if (userAvailable != null)
        //    {
        //        return Ok(new JwtService(_configuration).GenerateToken(
        //            userAvailable.Id.ToString(),
        //            userAvailable.Name,
        //            userAvailable.Email
        //            ));
        //    }
        //    return Ok("Failure");
        //}

        [HttpPost]
        public IActionResult Login([FromBody] Admin admin)
        {
            var userAvailable = Authenticate(admin);
            if (userAvailable != null)
            {
                var access_token = Generate(admin);
                return Ok(access_token);
            }
            return Ok("Failure");
        }

        private string Generate(Admin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtConfig:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email,admin.Email)
            };

            var access_token = new JwtSecurityToken(_configuration["jwtConfig:Issuer"],
                _configuration["jwtConfig:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(access_token);
        }

        private Admin? Authenticate(Admin admin)
        {
            var currentUser = _context.Admins.Where(u => u.Email == admin.Email && u.AdminPassword == admin.AdminPassword).FirstOrDefault();
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        [HttpGet("Blah")]
        [Authorize]
        public IActionResult Blah()
        {
            return Ok("Hellooooooooo");
        }
    }
}
