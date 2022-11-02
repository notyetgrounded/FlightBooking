using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EuroTrip2.Models
{
    public class JwtService
    {
        public string SecretKey { get; set; }
        public int TokenDuration { get; set; }

        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            this.SecretKey = _configuration.GetSection("jwtConfig").GetSection("Key").Value;
            this.TokenDuration = Int32.Parse(_configuration.GetSection("jwtConfig").GetSection("Duration").Value);
        }

        public string GenerateToken(string ID, string UserName, string EmailID)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.SecretKey));

            var signature = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var payLoad = new[]
            {
                new Claim("ID",ID),
                new Claim("UserName",UserName),
                new Claim("EmailID",EmailID)
            };

            var jwtToken = new JwtSecurityToken(
                issuer: "localhost",
                audience: "locahost",
                claims: payLoad,
                expires: DateTime.Now.AddMinutes(TokenDuration),
                signingCredentials: signature
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
