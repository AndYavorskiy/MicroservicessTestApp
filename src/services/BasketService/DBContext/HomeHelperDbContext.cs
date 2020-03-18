using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BasketService.Entities;
using Infrastructure.Models;

namespace BasketService.DBContext
{
    public class HomeHelperDbContext : IHomeHelperDbContext
    {
        private readonly IMongoDatabase _db;

        public HomeHelperDbContext(IOptions<MongoDBConfig> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            _db = client.GetDatabase(config.Value.Database);
        }

        public IMongoCollection<BasketItem> BasketItems => _db.GetCollection<BasketItem>("BasketItems");
    }
}
