using BasketService.DBContext;
using BasketService.Repositories;
using Infrastructure.Extensions;
using Infrastructure.Models;
using Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BasketService
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
            services.ConfigureSwagger("Basket Service HTTP API");

            services.AddRabbit(Configuration);

            services.Configure<MongoDBConfig>(Configuration.GetSection("MongoDB"));
            services.AddTransient<IHomeHelperDbContext, HomeHelperDbContext>();
            services.AddTransient<IBasketItemRepository, BasketItemRepository>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket Service API V1");
            });

            app.UseHttpsRedirection();

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
