using UserService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();

        Task<User> Get(string id);

        Task<User> Create(User user);

        Task<bool> Update(User user);

        Task<bool> Delete(string id);
    }
}
