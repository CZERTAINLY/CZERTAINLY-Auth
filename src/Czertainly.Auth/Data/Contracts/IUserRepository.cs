using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Data.Contracts
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetRoleUsersAsync(Guid roleUuid);
    }
}
