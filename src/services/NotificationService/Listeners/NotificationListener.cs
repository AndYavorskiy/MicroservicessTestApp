using Infrastructure.RabbitMQ.Listeners;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;

namespace NotificationService.Listeners
{
    public class NotificationListener : RabbitBaseListener<Notification>
    {
        private readonly ILogger<RabbitBaseListener<Notification>> _logger;

        // Because the Process function is a delegate callback, if you inject other services directly, they are not in one scope.
        // To invoke other Service instances, you can only use IServiceProvider CreateScope to retrieve instance objects
        private readonly IServiceProvider _services;

        public NotificationListener(IServiceProvider services,
                            IPooledObjectPolicy<IModel> pooledObjectPolicy,
                            ILogger<RabbitBaseListener<Notification>> logger) : base(pooledObjectPolicy)
        {
            _logger = logger;
            _services = services;

            ExchangeName = "base.exchange.topic";
            QueueName = "notifications";
            RouteKey = "notifications.create.#";
        }

        public override bool ProcessData(Notification message)
        {
            // Returning to false directly rejects this message, indicating that it cannot be processed
            if (message == null)
            {
                return false;
            }

            try
            {
                var obj = JsonConvert.SerializeObject(message);

                _logger.LogInformation($"Processed successfully: {obj}");

                //using (var scope = _services.CreateScope())
                //{
                //    var xxxService = scope.ServiceProvider.GetRequiredService<XXXXService>();
                //    return true;
                //}

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Process fail,error:{ex.Message},stackTrace:{ex.StackTrace}");
                _logger.LogError(-1, ex, "Process fail");
                return false;
            }
        }
    }
}
