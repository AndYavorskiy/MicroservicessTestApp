using Infrastructure.ServiceDiscovery;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class CommonExtension
    {
        public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = "http://localhost:54140";
            var audience = configuration.GetValue<string>("Audience");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.SaveToken = true;
                 x.TokenValidationParameters = TokenValidator.GetTokenValidationParameters();
                 x.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }

                         return Task.CompletedTask;
                     }
                 };
             });
        }

        public static void ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceConfig = configuration.GetServiceConfig();

            services.RegisterConsulServices(serviceConfig);
        }

        public static void ConfigureSwagger(this IServiceCollection services, string title)
        {
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = title,
                    });

                    c.AddSecurityDefinition("Bearer",
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Description = "Please enter into field the word 'Bearer' following by space and JWT",
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "IdentityApiKey",
                        });

                    var openApiSecurityScheme = new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme,
                        },
                    };

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        { openApiSecurityScheme, new List<string>() },
                    });
                });
        }
    }
}
