using SecondService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecondService.Repositories
{
    public interface IModuleRepository
    {
        Task<List<Module>> GetAllModules();

        Task<Module> GetModule(long id);

        Task<Module> Create(Module module);

        Task<bool> Update(Module module);

        Task<bool> Delete(long id);

        Task<long> GetNextId();
    }
}
