using MedicalService.DBContext;
using MedicalService.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalService.Repositories
{
    public class MedicamentsRepository : IMedicamentsRepository
    {
        private readonly IHomeHelperDbContext _context;

        public MedicamentsRepository(IHomeHelperDbContext context)
        {
            _context = context;
        }

        public Task<List<Medicaments>> GetAll(string userId)
        {
            return _context
                .Medicaments
                .Find(x => x.UserId == userId)
                .SortBy(x => x.Name)
                .ToListAsync();
        }


        public Task<Medicaments> Get(string id, string userId)
        {
            return _context
                    .Medicaments
                    .Find(s => s.Id == id && s.UserId == userId)
                    .FirstOrDefaultAsync();
        }

        public async Task<Medicaments> Create(Medicaments medicaments)
        {
            await _context.Medicaments.InsertOneAsync(medicaments);

            return medicaments;
        }

        public async Task<bool> Update(Medicaments medicaments)
        {
            var updateResult =
                await _context
                    .Medicaments
                    .ReplaceOneAsync(
                        filter: g => g.Id == medicaments.Id,
                        replacement: medicaments);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var deleteResult = await _context
                .Medicaments
                .DeleteOneAsync(x => x.Id == id);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}