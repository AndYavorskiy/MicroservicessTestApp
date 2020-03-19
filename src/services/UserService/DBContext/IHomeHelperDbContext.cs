using UserService.Entities;
using MongoDB.Driver;

namespace UserService.DBContext
{
    public interface IHomeHelperDbContext
    {
        IMongoCollection<User> Users { get; }
    }
}