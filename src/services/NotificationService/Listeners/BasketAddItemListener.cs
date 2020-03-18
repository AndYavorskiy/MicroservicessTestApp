using Infrastructure.RabbitMQ.Listeners;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using NotificationService.Models;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace NotificationService.Listeners
{
    public class BasketAddItemListener : RabbitBaseListener<BasketItemModel>
    {
        private readonly ILogger<RabbitBaseListener<BasketItemModel>> logger;

        // Because the Process function is a delegate callback, if you inject other services directly, they are not in one scope.
        // To invoke other Service instances, you can only use IServiceProvider CreateScope to retrieve instance objects
        private readonly IServiceProvider services;

        public BasketAddItemListener(IServiceProvider services,
                            IPooledObjectPolicy<IModel> pooledObjectPolicy,
                            ILogger<RabbitBaseListener<BasketItemModel>> logger) : base(pooledObjectPolicy)
        {
            this.logger = logger;
            this.services = services;

            ExchangeName = "base.exchange.topic";
            QueueName = "notifications";
            RouteKey = "notifications.basket.add.#";
        }

        public override Task<bool> ProcessData(BasketItemModel message)
        {
            // Returning to false directly rejects this message, indicating that it cannot be processed
            if (message == null)
            {
                return Task.FromResult(false);
            }

            try
            {
                logger.LogInformation($"Processed successfully ({RouteKey}): { JsonConvert.SerializeObject(message)}");

                //using (var scope = _services.CreateScope())
                //{
                //    var xxxService = scope.ServiceProvider.GetRequiredService<XXXXService>();
                //    return true;
                //}

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Process fail, error:{ex.Message}, stackTrace:{ex.StackTrace}");
                logger.LogError(-1, ex, "Process fail");
                return Task.FromResult(false);
            }
        }
    }
}
