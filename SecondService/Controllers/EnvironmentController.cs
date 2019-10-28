using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SecondService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<int> Get()
        {
            return Enumerable
                  .Range(1, 25)
                  .Select(i => new Random().Next(1, i));
        }
    }
}