using AuthorizationService.Entities;
using AuthorizationService.Models;
using AuthorizationService.Repositories;
using Infrastructure.Extensions;
using Infrastructure.RabbitMQ;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IUserRepository userRepository;
        private readonly IRabbitManager manager;
        private readonly AuthorizationConfigs authorizationConfigs;

        public AuthController(ILogger<AuthController> logger,
            IUserRepository userRepository,
            IRabbitManager manager,
            AuthorizationConfigs authorizationConfigs)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.manager = manager;
            this.authorizationConfigs = authorizationConfigs;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserTokenModel>> Login(AuthModel credentials)
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

            return Ok(GenerateToken(user));
        }

        [HttpGet("refresh")]
        public async Task<ActionResult<UserTokenModel>> GetRefreshedToken()
        {
            var user = await userRepository.Get(User.GetLoggedInUserId());
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(GenerateToken(user));
        }

        [HttpGet("ping")]
        [AllowAnonymous]
        public ActionResult Ping()
        {
            return Ok();
        }

        private UserTokenModel GenerateToken(User user)
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

            return new UserTokenModel
            {
                Token = tokenHandler.WriteToken(token),
                ExpiredIn = new DateTimeOffset(tokenDescriptor.Expires.Value).ToUnixTimeMilliseconds(),
            };
        }
    }
}
