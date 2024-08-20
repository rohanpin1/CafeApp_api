using Azure;
using CafeApp_api.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CafeApp_api.Service
{
    
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GetToken(AuthenticateUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,_configuration["JWT:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("ProjectName","CoffeeBarista"),
                new Claim("Username",user.Email),
                new Claim("Password",user.Password),
                new Claim("Is2FAEnabled",user.Is2FAEnabled.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var crdentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Audience"],
                    
                    claims,
                    expires: DateTime.UtcNow.AddDays(10),
                    signingCredentials: crdentials
                    );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return tokenValue;   
        }
    }
}
