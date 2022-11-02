using EuroTrip2.Contexts;
using EuroTrip2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

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

        [HttpPost("Login")]
        public IActionResult Login(Login user)
        {
            var userAvailable = _context.Registrations.Where(u => u.EmailID == user.EmailID && u.Password == user.Password).FirstOrDefault();
            if (userAvailable != null)
            {
                return Ok(new JwtService(_configuration).GenerateToken(
                    userAvailable.Id.ToString(),
                    userAvailable.UserName,
                    userAvailable.EmailID
                    ));
            }
            return Ok("Failure");
        }
    }
}