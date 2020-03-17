using Infrastructure.Models;
using MedicalService.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MedicalService.DBContext
{
    public class HomeHelperDbContext: IHomeHelperDbContext
    {
        private readonly IMongoDatabase _db;

        public HomeHelperDbContext(IOptions<MongoDBConfig> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            _db = client.GetDatabase(config.Value.Database);
        }

        public IMongoCollection<Medicaments> Medicaments => _db.GetCollection<Medicaments>("Medicaments");
    }
}
