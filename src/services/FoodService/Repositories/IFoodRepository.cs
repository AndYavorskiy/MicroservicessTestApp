using FoodService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodService.Repositories
{
    public interface IFoodRepository
    {
        Task<List<Food>> GetAll();

        Task<Food> Get(string id);

        Task<Food> Create(Food module);

        Task<bool> Update(Food module);

        Task<bool> Delete(string id);
    }
}
