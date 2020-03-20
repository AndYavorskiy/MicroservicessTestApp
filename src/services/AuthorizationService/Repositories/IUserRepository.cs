using AuthorizationService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationService.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();

        Task<User> Get(string id);

        Task<User> GetByEmailAsync(string email);

        Task<User> Create(User user);

        Task<bool> Update(User user);

        Task<bool> Delete(string id);
    }
}
