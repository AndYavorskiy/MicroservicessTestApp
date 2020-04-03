using BasketService.Entities;
using BasketService.Models;
using BasketService.Repositories;
using Infrastructure.Extensions;
using Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> logger;
        private readonly IBasketItemRepository basketItemRepository;
        private readonly IRabbitManager manager;

        public BasketController(ILogger<BasketController> logger,
            IBasketItemRepository basketItemRepository,
            IRabbitManager manager)
        {
            this.logger = logger;
            this.basketItemRepository = basketItemRepository;
            this.manager = manager;
        }


        [HttpGet]
        public async Task<ActionResult<List<BasketItemModel>>> GetAll()
        {
            return new ObjectResult((await basketItemRepository.GetAll(User.GetLoggedInUserId()))
                .Select(MapToModel)
                .ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BasketItemModel>> Get(string id)
        {
            var basketItem = await basketItemRepository.Get(id, User.GetLoggedInUserId());
            if (basketItem == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(MapToModel(basketItem));
        }

        [HttpPost]
        public async Task<ActionResult<BasketItemModel>> Create(BasketItemModel basketItemModel)
        {
            var basketItem = new BasketItem
            {
                Name = basketItemModel.Name,
                Amount = basketItemModel.Amount,
                Hint = basketItemModel.Hint,
                Description = basketItemModel.Description,
                UserId = User.GetLoggedInUserId(),
                DateCreated = basketItemModel.DateCreated,
                ItemType = basketItemModel.ItemType
            };

            await basketItemRepository.Create(basketItem);

            var result = MapToModel(basketItem);

            manager.Publish(
              message: result,
              exchangeName: "base.exchange.topic",
              exchangeType: ExchangeType.Topic,
              routeKey: "notifications.basket.add"
            );

            return new ObjectResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BasketItemModel>> Update(string id, [FromBody] BasketItemModel basketItemModel)
        {
            var basketItem = await basketItemRepository.Get(id, User.GetLoggedInUserId());
            if (basketItem == null)
            {
                return new NotFoundResult();
            }

            basketItem.Name = basketItemModel.Name;
            basketItem.Amount = basketItemModel.Amount;
            basketItem.Hint = basketItemModel.Hint;
            basketItem.Description = basketItemModel.Description;
            basketItem.DateCreated = basketItemModel.DateCreated;
            basketItem.ItemType = basketItemModel.ItemType;

            await basketItemRepository.Update(basketItem);

            return new ObjectResult(MapToModel(basketItem));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var food = await basketItemRepository.Get(id, User.GetLoggedInUserId());
            if (food == null)
            {
                return new NotFoundResult();
            }

            await basketItemRepository.Delete(id);

            return new NoContentResult();
        }

        [HttpPost("complete")]
        public async Task<ActionResult> CompletePurchase([FromBody] BasketCompletePurchaseModel completePurchaseModel)
        {
            var basketItem = await basketItemRepository.Get(completePurchaseModel.Id, User.GetLoggedInUserId());

            if (basketItem == null)
            {
                return new NotFoundResult();
            }

            await basketItemRepository.Delete(completePurchaseModel.Id);

            basketItem.Amount = completePurchaseModel.Amount;
            basketItem.ExpirationDate = completePurchaseModel.ExpirationDate;

            var itemTypeRouteKey = string.Empty;

            switch (basketItem.ItemType)
            {
                case Enums.BasketItemType.Medicaments:
                    itemTypeRouteKey = "medicaments";
                    break;
                case Enums.BasketItemType.Food:
                    itemTypeRouteKey = "food";
                    break;
            }

            //Add or Update available items
            manager.Publish(
              message: MapToModel(basketItem),
              exchangeName: "base.exchange.topic",
              exchangeType: ExchangeType.Topic,
              routeKey: $"purchase.{itemTypeRouteKey}"
            );

            //Notify about purchase
            //TODO: ...

            return Ok();
        }


        private static BasketItemModel MapToModel(BasketItem item) => new BasketItemModel
        {
            Id = item.Id,
            Name = item.Name,
            Amount = item.Amount,
            Description = item.Description,
            Hint = item.Hint,
            DateCreated = item.DateCreated,
            ItemType = item.ItemType,
            UserId = item.UserId
        };
    }
}
