using MedicalService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalService.Repositories
{
    public interface IMedicamentsRepository
    {
        Task<List<Medicaments>> GetAll(string userId);

        Task<Medicaments> Get(string id, string userId);

        Task<Medicaments> Create(Medicaments medicaments);

        Task<bool> Update(Medicaments medicaments);

        Task<bool> Delete(string id);
    }
}
