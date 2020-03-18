using BasketService.DBContext;
using BasketService.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketService.Repositories
{
    public class BasketItemRepository : IBasketItemRepository
    {
        private readonly IHomeHelperDbContext _context;

        public BasketItemRepository(IHomeHelperDbContext context)
        {
            _context = context;
        }

        public Task<List<BasketItem>> GetAll()
        {
            return _context
                .BasketItems
                .Find(x => true)
                .SortBy(x => x.Name)
                .ToListAsync();
        }


        public Task<BasketItem> Get(string id)
        {
            return _context
                    .BasketItems
                    .Find(s => s.Id == id)
                    .FirstOrDefaultAsync();
        }

        public async Task<BasketItem> Create(BasketItem basketItem)
        {
            await _context.BasketItems.InsertOneAsync(basketItem);

            return basketItem;
        }

        public async Task<bool> Update(BasketItem basketItem)
        {
            var updateResult =
                await _context
                    .BasketItems
                    .ReplaceOneAsync(
                        filter: g => g.Id == basketItem.Id,
                        replacement: basketItem);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var deleteResult = await _context
                .BasketItems
                .DeleteOneAsync(x => x.Id == id);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}