using AuthorizationService.DBContext;
using AuthorizationService.Listeners;
using AuthorizationService.Models;
using AuthorizationService.Repositories;
using Infrastructure.Extensions;
using Infrastructure.Middlewares;
using Infrastructure.Models;
using Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthorizationService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureConsul(Configuration);
            services.ConfigureAuthorization(Configuration);
            services.ConfigureSwagger("Authorization Service HTTP API");

            services.AddRabbit(Configuration);

            services.Configure<MongoDBConfig>(Configuration.GetSection("MongoDB"));
            services.AddTransient<IHomeHelperDbContext, HomeHelperDbContext>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddHostedService<UserCredentialsChangeListener>();

            var authorizationConfigs = new AuthorizationConfigs();
            Configuration.Bind("Authorization", authorizationConfigs);
            services.AddSingleton(authorizationConfigs);

            services.AddControllers();

            // configure jwt authentication
            //var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            //var audience = Configuration.GetValue<string>("Audience");

            //var key = Encoding.ASCII.GetBytes(authorizationConfigs.TokenKey);

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalErrorHandling();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization Service API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
