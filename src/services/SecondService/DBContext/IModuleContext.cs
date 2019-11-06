using MongoDB.Driver;
using SecondService.Models;

namespace SecondService.DBContext
{
    public interface IModuleContext
    {
        IMongoCollection<Module> Modules { get; }
    }
}
