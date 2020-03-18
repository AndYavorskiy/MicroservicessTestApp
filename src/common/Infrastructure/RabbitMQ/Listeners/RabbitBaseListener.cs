using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.RabbitMQ.Listeners
{
    public abstract class RabbitBaseListener<T> : IHostedService
    {
        private readonly IModel channel;

        protected string ExchangeName;
        protected string QueueName;
        protected string RouteKey;

        public RabbitBaseListener(IPooledObjectPolicy<IModel> pooledObjectPolicy)
        {
            channel = pooledObjectPolicy.Create();
        }

        public abstract Task<bool> ProcessData(T message);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }

        public void Register()
        {
            Console.WriteLine($"RabbitListener register, exchange: {ExchangeName}, queue: {QueueName}, routeKey:{RouteKey}");

            channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Topic);

            channel.QueueDeclare(queue: QueueName, exclusive: false, autoDelete: false);

            channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: RouteKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);
                var obj = JsonConvert.DeserializeObject<T>(message);

                var result = await ProcessData(obj);
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, true);
                }
            };

            channel.BasicConsume(queue: QueueName, consumer: consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            channel.Close();
            return Task.CompletedTask;
        }
    }
}
