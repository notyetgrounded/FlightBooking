using EuroTrip2.Contexts;
using EuroTrip2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EuroTrip2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginRegistrationController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public readonly FlightDBContext _context;
        public UserLoginRegistrationController(IConfiguration configuration, FlightDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(Registration user)
        {
            if (_context.Registrations.Where(u => u.EmailID == user.EmailID).FirstOrDefault() != null)
            {
                return Ok("AlreadyExist");
            }
            _context.Registrations.Add(user);
            _context.SaveChanges();
            return Ok("Success");
        }

        //[HttpPost("Login")]
        //public IActionResult Login(Login user)
        //{
        //    var userAvailable = _context.Registrations.Where(u => u.EmailID == user.EmailID && u.Password == user.Password).FirstOrDefault();
        //    if (userAvailable != null)
        //    {
        //        return Ok(new JwtService(_configuration).GenerateToken(
        //            userAvailable.Id.ToString(),
        //            userAvailable.UserName,
        //            userAvailable.EmailID
        //            ));
        //    }
        //    return Ok("Failure");
        //}

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login user)
        {
            var userAvailable = Authenticate(user);
            if (userAvailable != null)
            {
                var access_token = Generate(user);
                return Ok(access_token);
            }
            return Ok("Failure");
        }

        private string Generate(Login user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtConfig:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.EmailID)
            };

            var access_token = new JwtSecurityToken(_configuration["jwtConfig:Issuer"],
                _configuration["jwtConfig:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(access_token);
        }

        private Registration? Authenticate(Login user)
        {
            var currentUser = _context.Registrations.Where(u => u.EmailID == user.EmailID && u.Password == user.Password).FirstOrDefault();
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