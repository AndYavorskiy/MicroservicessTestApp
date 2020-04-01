using BasketService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketService.Repositories
{
    public interface IBasketItemRepository
    {
        Task<List<BasketItem>> GetAll(string userId);

        Task<BasketItem> Get(string id, string userId);

        Task<BasketItem> Create(BasketItem module);

        Task<bool> Update(BasketItem module);

        Task<bool> Delete(string id);
    }
}
