using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FoodService.Models;
using FoodService.Entities;
using Infrastructure.Models;

namespace FoodService.DBContext
{
    public class HomeHelperDbContext: IHomeHelperDbContext
    {
        private readonly IMongoDatabase _db;

        public HomeHelperDbContext(IOptions<MongoDBConfig> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            _db = client.GetDatabase(config.Value.Database);
        }

        public IMongoCollection<Food> Food => _db.GetCollection<Food>("Food");
    }
}
