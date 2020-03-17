using FoodService.Repositories;
using MedicalService.Entities;
using MedicalService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicamentsController : ControllerBase
    {
        private readonly ILogger<MedicamentsController> logger;
        private readonly IMedicamentsRepository medicamentsRepository;

        public MedicamentsController(IMedicamentsRepository medicamentsRepository, ILogger<MedicamentsController> logger)
        {
            this.medicamentsRepository = medicamentsRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<MedicamentsModel>>> Get()
        {
            return new ObjectResult((await medicamentsRepository.GetAll())
                .Select(MapToModel)
                .ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicamentsModel>> Get(string id)
        {
            var food = await medicamentsRepository.Get(id);
            if (food == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(MapToModel(food));
        }

        [HttpPost]
        public async Task<ActionResult<MedicamentsModel>> Create(MedicamentsModel foodModel)
        {
            var food = new Medicaments
            {
                Name = foodModel.Name,
                Amount = foodModel.Amount,
                Description = foodModel.Description,
                ExpirationDate = foodModel.ExpirationDate,
                UserId = new Guid()
            };

            await medicamentsRepository.Create(food);

            return new ObjectResult(MapToModel(food));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MedicamentsModel>> Update(string id, [FromBody] MedicamentsModel module)
        {
            var food = await medicamentsRepository.Get(id);
            if (food == null)
            {
                return new NotFoundResult();
            }

            food.Name = module.Name;
            food.Amount = module.Amount;
            food.Description = module.Description;
            food.ExpirationDate = module.ExpirationDate;

            await medicamentsRepository.Update(food);

            return new ObjectResult(MapToModel(food));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var food = await medicamentsRepository.Get(id);
            if (food == null)
            {
                return new NotFoundResult();
            }

            await medicamentsRepository.Delete(id);

            return new NoContentResult();
        }

        private static MedicamentsModel MapToModel(Medicaments medicaments) => new MedicamentsModel
        {
            Id = medicaments.Id,
            Name = medicaments.Name,
            Amount = medicaments.Amount,
            Description = medicaments.Description,
            ExpirationDate = medicaments.ExpirationDate,
            UserId = medicaments.UserId
        };
    }
}
