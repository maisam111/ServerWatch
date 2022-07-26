using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServerAuth.Services
{
    public class AuthenticationService
    {
        private UserService _userService;
        public AuthenticationService(UserService userService)
        {
            _userService = userService;
        }
        public async Task<string> AuthenticateAsync(string email, string password)
        {
            if (await _userService.IsValidCredintialsAsync(email, password))
            {
                return GenerateToken(email);
            }
            else
            {
                return "";
            }

        }
        private string GenerateToken(string email)
        {
            var tokenKey = "private key secret";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var id = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, email),
            });
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = id,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
