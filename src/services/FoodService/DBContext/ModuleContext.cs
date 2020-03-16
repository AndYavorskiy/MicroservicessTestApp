using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FoodService.Models;

namespace FoodService.DBContext
{
    public class ModuleContext: IModuleContext
    {
        private readonly IMongoDatabase _db;

        public ModuleContext(IOptions<MongoDBConfig> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            _db = client.GetDatabase(config.Value.Database);
        }

        public IMongoCollection<Module> Modules => _db.GetCollection<Module>("Modules");
    }
}
