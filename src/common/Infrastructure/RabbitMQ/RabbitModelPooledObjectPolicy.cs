using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.RabbitMQ
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitMQConfig options;
        private readonly IConnection connection;

        public RabbitModelPooledObjectPolicy(IOptions<RabbitMQConfig> optionsAccs)
        {
            options = optionsAccs.Value;
            connection = GetConnection();
        }

        public IModel Create()
            => connection.CreateModel();


        public bool Return(IModel obj)
        {
            if (!obj.IsOpen)
            {
                obj?.Dispose();
            }

            return obj.IsOpen;
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = options.HostName,
                UserName = options.UserName,
                Password = options.Password,
                Port = options.Port,
                VirtualHost = options.VHost
            };

            return factory.CreateConnection();
        }
    }
}
