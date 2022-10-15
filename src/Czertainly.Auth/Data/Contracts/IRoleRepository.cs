using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Data.Contracts
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<IEnumerable<Role>> GetUserRolesAsync(Guid userUuid);
    }
}
