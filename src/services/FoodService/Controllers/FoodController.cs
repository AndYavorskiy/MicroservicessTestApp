using Microsoft.AspNetCore.Mvc;
using FoodService.Models;
using FoodService.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodService.Entities;
using System;
using MongoDB.Bson;

namespace FoodService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository moduleRepository;

        public FoodController(IFoodRepository moduleRepository)
        {
            this.moduleRepository = moduleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<FoodModel>>> Get()
        {
            return new ObjectResult(await moduleRepository.GetAllModules());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodModel>> Get(long id)
        {
            var todo = await moduleRepository.GetModule(id);
            if (todo == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(todo);
        }

        [HttpPost]
        public async Task<ActionResult<FoodModel>> Post(FoodModel module)
        {
            var food = new Food
            {
                Id = await moduleRepository.GetNextId(),
                Name = module.Name,
                Amount = module.Amount,
                Description = module.Description,
                ExpirationDate = module.ExpirationDate,
                UserId = new Guid()
            };

            await moduleRepository.Create(food);

            return new OkObjectResult(module);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FoodModel>> Put(long id, [FromBody] FoodModel module)
        {
            var food = await moduleRepository.GetModule(id);
            if (food == null)
            {
                return new NotFoundResult();
            }

            food.Name = module.Name;
            food.Amount = module.Amount;
            food.Description = module.Description;
            food.ExpirationDate = module.ExpirationDate;

            await moduleRepository.Update(food);

            return new OkObjectResult(module);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var post = await moduleRepository.GetModule(id);
            if (post == null)
            {
                return new NotFoundResult();
            }

            await moduleRepository.Delete(id);

            return new OkResult();
        }
    }
}