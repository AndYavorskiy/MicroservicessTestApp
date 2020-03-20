using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AuthorizationService.Entities;
using Infrastructure.Models;

namespace AuthorizationService.DBContext
{
    public class HomeHelperDbContext : IHomeHelperDbContext
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
