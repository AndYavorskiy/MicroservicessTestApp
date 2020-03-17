using MedicalService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodService.Repositories
{
    public interface IMedicamentsRepository
    {
        Task<List<Medicaments>> GetAll();

        Task<Medicaments> Get(string id);

        Task<Medicaments> Create(Medicaments medicaments);

        Task<bool> Update(Medicaments medicaments);

        Task<bool> Delete(string id);
    }
}
