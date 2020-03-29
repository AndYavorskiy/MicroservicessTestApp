using AuthorizationService.Entities;
using MongoDB.Driver;

namespace AuthorizationService.DBContext
{
    public interface IHomeHelperDbContext
    {
        IMongoCollection<User> Users { get; }

        IMongoCollection<RefreshToken> RefreshTokens { get; }
    }
}