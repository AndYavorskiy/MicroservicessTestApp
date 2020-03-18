using FoodService.Entities;
using FoodService.Models;
using FoodService.Repositories;
using Infrastructure.RabbitMQ.Listeners;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace FoodService.Listeners
{
    public class PurchaseFoodListener : RabbitBaseListener<BasketItemModel>
    {
        private readonly ILogger<RabbitBaseListener<BasketItemModel>> logger;

        // Because the Process function is a delegate callback, if you inject other services directly, they are not in one scope.
        // To invoke other Service instances, you can only use IServiceProvider CreateScope to retrieve instance objects
        private readonly IServiceProvider services;

        public PurchaseFoodListener(IServiceProvider services,
                            IPooledObjectPolicy<IModel> pooledObjectPolicy,
                            ILogger<RabbitBaseListener<BasketItemModel>> logger) : base(pooledObjectPolicy)
        {
            this.logger = logger;
            this.services = services;

            ExchangeName = "base.exchange.topic";
            QueueName = "purchase-food";
            RouteKey = "purchase.food.#";
        }

        public override async Task<bool> ProcessData(BasketItemModel message)
        {
            // Returning to false directly rejects this message, indicating that it cannot be processed
            if (message == null)
            {
                return false;
            }

            try
            {
                logger.LogInformation($"Processed successfully ({RouteKey}): { JsonConvert.SerializeObject(message)}");

                using (var scope = services.CreateScope())
                {
                    var foodRepository = scope.ServiceProvider.GetRequiredService<IFoodRepository>();

                    await foodRepository.Create(new Food()
                    {
                        Name = message.Name,
                        Amount = message.Amount,
                        Description = message.Description,
                        UserId = message.UserId,
                        ExpirationDate = message.ExpirationDate
                    });
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Process fail, error:{ex.Message}, stackTrace:{ex.StackTrace}");
                logger.LogError(-1, ex, "Process fail");
                return false;
            }
        }
    }
}
