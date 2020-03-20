using AuthorizationService.Entities;
using AuthorizationService.Models;
using AuthorizationService.Repositories;
using Infrastructure.RabbitMQ.Listeners;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace AuthorizationService.Listeners
{
    public class UserCredentialsChangeListener : RabbitBaseListener<UserModel>
    {
        private readonly ILogger<RabbitBaseListener<UserModel>> logger;

        private readonly IServiceProvider services;

        public UserCredentialsChangeListener(IServiceProvider services,
                            IPooledObjectPolicy<IModel> pooledObjectPolicy,
                            ILogger<RabbitBaseListener<UserModel>> logger) : base(pooledObjectPolicy)
        {
            this.logger = logger;
            this.services = services;

            ExchangeName = "base.exchange.topic";
            QueueName = "purchase-food";
            RouteKey = "user.#";
        }

        public override async Task<bool> ProcessData(UserModel message)
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
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    var user = await userRepository.Get(message.Id);

                    var isNewUser = user == null;

                    user ??= new User()
                    {
                        Id = message.Id
                    };

                    user.Email = message.Email;
                    user.Password = message.Password;
                    user.IsActive = message.IsActive;

                    if (isNewUser)
                    {
                        await userRepository.Create(user);
                    }
                    else
                    {
                        await userRepository.Update(user);
                    }
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
