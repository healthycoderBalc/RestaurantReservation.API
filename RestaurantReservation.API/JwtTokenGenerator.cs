using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.API.Models.Authentication;
using RestaurantReservation.Db.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestaurantReservation.API
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GenerateToken(Employee user)
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.EmployeeId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));

            var jwtSecurityToken = new JwtSecurityToken(
              _configuration["Authentication:Issuer"],
              _configuration["Authentication:Audience"],
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
              .WriteToken(jwtSecurityToken);

            return tokenToReturn;
        }
    }
}
