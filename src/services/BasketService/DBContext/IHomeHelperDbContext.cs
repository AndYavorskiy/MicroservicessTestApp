using BasketService.Entities;
using MongoDB.Driver;

namespace BasketService.DBContext
{
    public interface IHomeHelperDbContext
    {
        IMongoCollection<BasketItem> BasketItems { get; }
    }
}