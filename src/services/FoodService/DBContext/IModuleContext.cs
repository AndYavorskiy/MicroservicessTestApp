using MongoDB.Driver;
using FoodService.Models;

namespace FoodService.DBContext
{
    public interface IModuleContext
    {
        IMongoCollection<Module> Modules { get; }
    }
}
