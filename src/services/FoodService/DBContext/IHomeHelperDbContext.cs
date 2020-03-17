using FoodService.Entities;
using MongoDB.Driver;

namespace FoodService.DBContext
{
    public interface IHomeHelperDbContext
    {
        IMongoCollection<Food> Food { get; }
    }
}