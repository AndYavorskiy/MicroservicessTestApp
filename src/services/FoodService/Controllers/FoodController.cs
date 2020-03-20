using FoodService.Entities;
using FoodService.Models;
using FoodService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository foodRepository;

        public FoodController(IFoodRepository foodRepository)
        {
            this.foodRepository = foodRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<FoodModel>>> Get()
        {
            return new ObjectResult((await foodRepository.GetAll())
                .Select(MapToModel)
                .ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodModel>> Get(string id)
        {
            var food = await foodRepository.Get(id);
            if (food == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(MapToModel(food));
        }

        [HttpPost]
        public async Task<ActionResult<FoodModel>> Create(FoodModel foodModel)
        {
            var food = new Food
            {
                Name = foodModel.Name,
                Amount = foodModel.Amount,
                Description = foodModel.Description,
                ExpirationDate = foodModel.ExpirationDate,
                UserId = new Guid()
            };

            await foodRepository.Create(food);

            return new ObjectResult(MapToModel(food));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FoodModel>> Update(string id, [FromBody] FoodModel module)
        {
            var food = await foodRepository.Get(id);
            if (food == null)
            {
                return new NotFoundResult();
            }

            food.Name = module.Name;
            food.Amount = module.Amount;
            food.Description = module.Description;
            food.ExpirationDate = module.ExpirationDate;

            await foodRepository.Update(food);

            return new ObjectResult(MapToModel(food));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var food = await foodRepository.Get(id);
            if (food == null)
            {
                return new NotFoundResult();
            }

            await foodRepository.Delete(id);

            return new NoContentResult();
        }

        private static FoodModel MapToModel(Food food) => new FoodModel
        {

            Id = food.Id,
            Name = food.Name,
            Amount = food.Amount,
            Description = food.Description,
            ExpirationDate = food.ExpirationDate,
            UserId = food.UserId
        };
    }
}