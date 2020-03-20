using Infrastructure.ServiceDiscovery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Extensions
{
    public static class CommonExtension
    {
        public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var authenticationScheme = "IdentityApiKey";
            var identityUrl = configuration.GetValue<string>("IdentityUrl");
            var audience = configuration.GetValue<string>("Audience");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(authenticationScheme, options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = audience;
            });
        }

        public static void ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceConfig = configuration.GetServiceConfig();

            services.RegisterConsulServices(serviceConfig);
        }
    }
}
