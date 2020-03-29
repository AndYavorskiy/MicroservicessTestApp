using AuthorizationService.Entities;
using AuthorizationService.Models;
using AuthorizationService.Repositories;
using Infrastructure.Extensions;
using Infrastructure.RabbitMQ;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IUserRepository userRepository;
        private readonly IRabbitManager manager;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly AuthorizationConfigs authorizationConfigs;

        public AuthController(ILogger<AuthController> logger,
            IUserRepository userRepository,
            IRabbitManager manager,
            IRefreshTokenRepository refreshTokenRepository,
            AuthorizationConfigs authorizationConfigs)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.manager = manager;
            this.refreshTokenRepository = refreshTokenRepository;
            this.authorizationConfigs = authorizationConfigs;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthTokenModel>> Login(AuthModel credentials)
        {
            var user = await userRepository.GetByEmailAsync(credentials.Login);
            if (user == null)
            {
                return Unauthorized();
            }

            if (!SecurePasswordHasher.Verify(credentials.Password, user.Password))
            {
                return Unauthorized();
            }

            var token = await GenerateToken(user);

            return Ok(token);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthTokenModel>> Refresh(RefreshTokenModel refreshTokenModel)
        {
            var principal = GetPrincipalFromExpiredToken(refreshTokenModel.Token);
            var userId = principal.GetLoggedInUserId(); ;

            if (string.IsNullOrEmpty(userId))
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var user = await userRepository.Get(userId);
            var oldRefreshToken = await refreshTokenRepository.Get(userId, refreshTokenModel.RefreshToken);

            if (user == null || oldRefreshToken == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            await refreshTokenRepository.Delete(userId, oldRefreshToken.Id);

            var newToken = await GenerateToken(user);

            return Ok(newToken);
        }

        private async Task<AuthTokenModel> GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(authorizationConfigs.TokenKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(authorizationConfigs.TokenExpiratinInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();

            await refreshTokenRepository.Create(new RefreshToken()
            {
                UserId = user.Id,
                Token = refreshToken
            });

            return new AuthTokenModel
            {
                Token = tokenHandler.WriteToken(token),
                ExpiredIn = new DateTimeOffset(tokenDescriptor.Expires.Value).ToUnixTimeMilliseconds(),
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = TokenValidator.GetTokenValidationParameters();

            //here we are saying that we don't care about the token's expiration date
            tokenValidationParameters.ValidateLifetime = false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
