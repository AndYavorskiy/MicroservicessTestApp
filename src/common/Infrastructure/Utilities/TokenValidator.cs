using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Utilities
{
    public class TokenValidator
    {
        public static TokenValidationParameters GetTokenValidationParameters()
        {
            var key = Encoding.ASCII.GetBytes("my-secure-token-key-that-is-very-secure");

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
        }
    }
}
