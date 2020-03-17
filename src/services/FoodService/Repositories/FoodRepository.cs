using MongoDB.Bson;
using MongoDB.Driver;
using FoodService.DBContext;
using FoodService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodService.Entities;

namespace FoodService.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly IModuleContext _context;

        public FoodRepository(IModuleContext context)
        {
            _context = context;
        }

        public Task<List<Food>> GetAllModules()
        {
            return _context
                .Food
                .Find(x => true)
                .SortBy(x => x.Name)
                .ToListAsync();
        }


        public Task<Food> GetModule(long id)
        {
            return _context
                    .Food
                    .Find(s => s.Id == id)
                    .FirstOrDefaultAsync();
        }

        public async Task<Food> Create(Food todo)
        {
            await _context.Food.InsertOneAsync(todo);

            return todo;
        }

        public async Task<bool> Update(Food todo)
        {
            var updateResult =
                await _context
                    .Food
                    .ReplaceOneAsync(
                        filter: g => g.Id == todo.Id,
                        replacement: todo);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var deleteResult = await _context
                .Food
                .DeleteOneAsync(m => m.Id == id);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<long> GetNextId()
        {
            return await _context.Food.CountDocumentsAsync(new BsonDocument()) + 1;
        }
    }
}