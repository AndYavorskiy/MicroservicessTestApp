using FoodService.Entities;
using MongoDB.Driver;

namespace FoodService.DBContext
{
    public interface IModuleContext
    {
        IMongoCollection<Food> Food { get; }
    }
}