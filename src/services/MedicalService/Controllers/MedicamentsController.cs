using MedicalService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicamentsController : ControllerBase
    {
        private readonly ILogger<MedicamentsController> _logger;

        public MedicamentsController(ILogger<MedicamentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<MedicamentsModel> Getll()
        {
            return Enumerable.Range(1, 5).Select(index => new MedicamentsModel
            {
                Id = index,
                Name = $"Name-{index}",
                Description = $"Description-{index}",
                ExpirationDate = DateTimeOffset.Now
            })
            .ToList();
        }

        [HttpGet("{id}")]
        public MedicamentsModel Get(int id)
        {
            return new MedicamentsModel()
            {
                Id = id,
                Name = $"Name-{id}",
                Description = $"Description-{id}",
                ExpirationDate = DateTimeOffset.Now
            };
        }
    }
}
