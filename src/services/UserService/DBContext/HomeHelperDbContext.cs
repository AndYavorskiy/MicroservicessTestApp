using Infrastructure.Models;
using UserService.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace UserService.DBContext
{
    public class HomeHelperDbContext: IHomeHelperDbContext
    {
        private readonly IMongoDatabase _db;

        public HomeHelperDbContext(IOptions<MongoDBConfig> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            _db = client.GetDatabase(config.Value.Database);
        }

        public IMongoCollection<User> Users => _db.GetCollection<User>("Users");
    }
}
