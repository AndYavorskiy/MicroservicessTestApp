using Infrastructure.RabbitMQ.Listeners;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using MedicalService.Models;
using RabbitMQ.Client;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using MedicalService.Entities;
using MedicalService.Repositories;

namespace MedicalService.Listeners
{
    public class PurchaseMedicamentsListener : RabbitBaseListener<BasketItemModel>
    {
        private readonly ILogger<RabbitBaseListener<BasketItemModel>> logger;

        // Because the Process function is a delegate callback, if you inject other services directly, they are not in one scope.
        // To invoke other Service instances, you can only use IServiceProvider CreateScope to retrieve instance objects
        private readonly IServiceProvider services;

        public PurchaseMedicamentsListener(IServiceProvider services,
                            IPooledObjectPolicy<IModel> pooledObjectPolicy,
                            ILogger<RabbitBaseListener<BasketItemModel>> logger) : base(pooledObjectPolicy)
        {
            this.logger = logger;
            this.services = services;

            ExchangeName = "base.exchange.topic";
            QueueName = "purchase-medicaments";
            RouteKey = "purchase.medicaments.#";
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
                    var medicamentsRepository = scope.ServiceProvider.GetRequiredService<IMedicamentsRepository>();

                    await medicamentsRepository.Create(new Medicaments()
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
