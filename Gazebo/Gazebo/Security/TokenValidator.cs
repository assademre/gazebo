
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gazebo.Security
{
    public interface ITokenValidator
    {
        ClaimsPrincipal ValidateToken(string token);
    }
    public class TokenValidator : ITokenValidator
    {
        public readonly IConfiguration _configuration;

        public TokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GetSecretKey())),
                ValidateIssuer = true,
                ValidIssuer = GetIssuer(),
                ValidateAudience = true,
                ValidAudience = GetIssuer(),
                RequireExpirationTime = true,
                ValidateLifetime = true
            };

            SecurityToken validatedToken;
            return tokenHandler.ValidateToken(token, validationParameters, out validatedToken) as ClaimsPrincipal;
        }

        public string GetSecretKey()
        {
            return _configuration["Jwt:SigningKey"] ?? string.Empty;
        }

        public string GetIssuer()
        {
            return _configuration["Jwt:Issuer"] ?? string.Empty;
        }
    }
}
