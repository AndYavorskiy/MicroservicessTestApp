using AuthorizationService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationService.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> Get(string userId, string token);

        Task<RefreshToken> Create(RefreshToken token);

        Task<bool> Delete(string userId, string tokenId);
    }
}
