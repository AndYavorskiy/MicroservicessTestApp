using FoodService.DBContext;
using FoodService.Listeners;
using FoodService.Repositories;
using Infrastructure.Models;
using Infrastructure.RabbitMQ;
using Infrastructure.ServiceDiscovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FoodService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureConsul(services);

            services.AddRabbit(Configuration);

            services.Configure<MongoDBConfig>(Configuration.GetSection("MongoDB"));
            services.AddScoped<IHomeHelperDbContext, HomeHelperDbContext>();
            services.AddScoped<IFoodRepository, FoodRepository>();

            services.AddHostedService<PurchaseFoodListener>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Food Service HTTP API"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Food Service API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureConsul(IServiceCollection services)
        {
            var serviceConfig = Configuration.GetServiceConfig();

            services.RegisterConsulServices(serviceConfig);
        }
    }
}
