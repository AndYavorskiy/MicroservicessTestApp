namespace Infrastructure.RabbitMQ
{
    public class RabbitMQConfig
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string VHost { get; set; }

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                {
                    return $@"amqp://{HostName}:{Port}/{VHost}";
                }

                return $@"amqp://{UserName}:{Password}@{HostName}:{Port}/{VHost}";
            }
        }
    }
}