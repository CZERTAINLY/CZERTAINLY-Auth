using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Data.Contracts
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<List<Permission>> GetRolePermissions(Guid roleUuid);
        Task<List<Permission>> GetRoleResourcePermissions(Guid roleUuid, Guid resourceUuid);
        Task<List<Permission>> GetUserPermissions(Guid userUuid);
    }
}
