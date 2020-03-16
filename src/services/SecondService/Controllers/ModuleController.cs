using Microsoft.AspNetCore.Mvc;
using SecondService.Models;
using SecondService.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecondService.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleRepository moduleRepository;

        public ModuleController(IModuleRepository moduleRepository)
        {
            this.moduleRepository = moduleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Module>>> Get()
        {
            return new ObjectResult(await moduleRepository.GetAllModules());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> Get(long id)
        {
            var todo = await moduleRepository.GetModule(id);
            if (todo == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(todo);
        }

        [HttpPost]
        public async Task<ActionResult<Module>> Post(Module module)
        {
            module.Id = await moduleRepository.GetNextId();
            await moduleRepository.Create(module);

            return new OkObjectResult(module);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Module>> Put(long id, [FromBody] Module module)
        {
            var moduleFromDb = await moduleRepository.GetModule(id);
            if (moduleFromDb == null)
            {
                return new NotFoundResult();
            }

            module.Id = moduleFromDb.Id;
            module.InternalId = moduleFromDb.InternalId;
            await moduleRepository.Update(module);

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