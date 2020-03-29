using AuthorizationService.DBContext;
using AuthorizationService.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationService.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IHomeHelperDbContext _context;

        public RefreshTokenRepository(IHomeHelperDbContext context)
        {
            _context = context;
        }

        public Task<RefreshToken> Get(string userId, string token)
        {
            return _context
                .RefreshTokens
                .Find(x => x.UserId == userId && x.Token == token)
                .SortBy(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<RefreshToken> Create(RefreshToken token)
        {
            await _context.RefreshTokens.InsertOneAsync(token);

            return token;
        }

        public async Task<bool> Delete(string userId, string tokenId)
        {
            var deleteResult = await _context
                .RefreshTokens
                .DeleteOneAsync(x => x.UserId == userId && x.Id == tokenId);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}