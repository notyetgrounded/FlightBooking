using EuroTrip2.Contexts;
using EuroTrip2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public IActionResult Login([FromBody] Admin user)
        {
            var userAvailable = _context.Admins.Where(u => u.Email == user.Email && u.AdminPassword == user.AdminPassword).FirstOrDefault();
            if (userAvailable != null)
            {
                return Ok(new JwtService(_configuration).GenerateToken(
                    userAvailable.Id.ToString(),
                    userAvailable.Name,
                    userAvailable.Email
                    ));
            }
            return Ok("Failure");
        }
    }
}
