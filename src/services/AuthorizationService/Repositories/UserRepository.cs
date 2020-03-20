using AuthorizationService.DBContext;
using AuthorizationService.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IHomeHelperDbContext _context;

        public UserRepository(IHomeHelperDbContext context)
        {
            _context = context;
        }

        public Task<List<User>> GetAll()
        {
            return _context
                .Users
                .Find(x => true)
                .SortBy(x => x.Id)
                .ToListAsync();
        }


        public Task<User> Get(string id)
        {
            return _context
                    .Users
                    .Find(s => s.Id == id)
                    .FirstOrDefaultAsync();
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return _context
                 .Users
                 .Find(s => s.Email == email)
                 .FirstOrDefaultAsync();
        }

        public async Task<User> Create(User user)
        {
            await _context.Users.InsertOneAsync(user);

            return user;
        }

        public async Task<bool> Update(User user)
        {
            var updateResult =
                await _context
                    .Users
                    .ReplaceOneAsync(
                        filter: g => g.Id == user.Id,
                        replacement: user);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var deleteResult = await _context
                .Users
                .DeleteOneAsync(x => x.Id == id);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}