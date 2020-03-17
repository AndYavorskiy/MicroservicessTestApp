using MedicalService.Entities;
using MongoDB.Driver;

namespace MedicalService.DBContext
{
    public interface IHomeHelperDbContext
    {
        IMongoCollection<Medicaments> Medicaments { get; }
    }
}