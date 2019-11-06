using MongoDB.Bson;
using MongoDB.Driver;
using SecondService.DBContext;
using SecondService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecondService.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly IModuleContext _context;

        public ModuleRepository(IModuleContext context)
        {
            _context = context;
        }

        public Task<List<Module>> GetAllModules()
        {
            return _context
                .Modules
                .Find(x => true)
                .SortBy(x => x.Id)
                .ToListAsync();
        }


        public Task<Module> GetModule(long id)
        {
            return _context
                    .Modules
                    .Find(s => s.Id == id)
                    .FirstOrDefaultAsync();
        }

        public async Task<Module> Create(Module todo)
        {
            await _context.Modules.InsertOneAsync(todo);

            return todo;
        }

        public async Task<bool> Update(Module todo)
        {
            var updateResult =
                await _context
                    .Modules
                    .ReplaceOneAsync(
                        filter: g => g.Id == todo.Id,
                        replacement: todo);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var deleteResult = await _context
                .Modules
                .DeleteOneAsync(m => m.Id == id);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<long> GetNextId()
        {
            return await _context.Modules.CountDocumentsAsync(new BsonDocument()) + 1;
        }
    }
}