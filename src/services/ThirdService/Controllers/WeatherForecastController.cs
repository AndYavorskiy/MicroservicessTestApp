using Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ThirdService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;
        private readonly IRedisClientsManager redisClientsManager;
        private readonly IRabbitManager manager;

        private readonly string WeatherListKey = "weather-forecast-list";

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IRedisClientsManager redisClientsManager,
            IRabbitManager manager)
        {
            this.logger = logger;
            this.redisClientsManager = redisClientsManager;
            this.manager = manager;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return GetList().GetAll();
        }

        [HttpGet("{weatherForecastId}")]
        public IActionResult Get(Guid weatherForecastId)
        {
            if (weatherForecastId == Guid.Empty)
            {
                return new BadRequestResult();
            }

            var cechedItem = GetList()
                .FirstOrDefault(x => x.Id == weatherForecastId);

            if (cechedItem == null)
            {
                return new BadRequestResult();
            }

            return Ok(cechedItem);
        }

        [HttpPost]
        public IActionResult Post(WeatherForecast weatherForecast)
        {
            if (weatherForecast == null)
            {
                return new BadRequestResult();
            }

            weatherForecast.Id = Guid.NewGuid();

            GetList().Add(weatherForecast);

            manager.Publish(
                message: weatherForecast,
                exchangeName: "base.exchange.topic",
                exchangeType: ExchangeType.Topic,
                routeKey: "notifications.create"
            );

            return Ok(weatherForecast);
        }

        [HttpPut]
        public IActionResult Put(WeatherForecast weatherForecast)
        {
            if (weatherForecast == null || weatherForecast.Id == Guid.Empty)
            {
                return new BadRequestResult();
            }

            var list = GetList();

            var cechedItem = list
                .FirstOrDefault(x => x.Id == weatherForecast.Id);

            if (cechedItem == null)
            {
                return new BadRequestResult();
            }

            list.Remove(cechedItem);
            list.Add(weatherForecast);

            return Ok(weatherForecast);
        }

        [HttpDelete]
        public IActionResult Delete(Guid weatherForecastId)
        {
            if (weatherForecastId == Guid.Empty)
            {
                return new BadRequestResult();
            }

            var list = GetList();

            var cechedItem = list
                .FirstOrDefault(x => x.Id == weatherForecastId);

            if (cechedItem == null)
            {
                return new BadRequestResult();
            }

            redisClientsManager.GetClient()
                .As<WeatherForecast>()
                .RemoveItemFromList(GetList(), cechedItem);

            return Ok(weatherForecastId);
        }

        private IRedisList<WeatherForecast> GetList()
            => redisClientsManager.GetClient()
                .As<WeatherForecast>()
                .Lists[WeatherListKey];
    }
}
