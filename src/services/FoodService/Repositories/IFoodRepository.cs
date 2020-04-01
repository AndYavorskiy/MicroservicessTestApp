using FoodService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodService.Repositories
{
    public interface IFoodRepository
    {
        Task<List<Food>> GetAll(string userId);

        Task<Food> Get(string id, string userId);

        Task<Food> Create(Food food);

        Task<bool> Update(Food food);

        Task<bool> Delete(string id);
    }
}
