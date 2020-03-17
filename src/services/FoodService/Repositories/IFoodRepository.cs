using FoodService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodService.Repositories
{
    public interface IFoodRepository
    {
        Task<List<Food>> GetAllModules();

        Task<Food> GetModule(long id);

        Task<Food> Create(Food module);

        Task<bool> Update(Food module);

        Task<bool> Delete(long id);

        Task<long> GetNextId();
    }
}
