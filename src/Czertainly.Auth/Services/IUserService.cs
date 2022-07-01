using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IUserService : IResourceService<UserDto>
    {
        //Task<List<RoleDto>> GetUserRoles(IEntityKey key);
        Task<UserDto> AssignRole(IEntityKey userKey, IEntityKey roleKey);
        Task<UserDto> AssignRoles(IEntityKey userKey, List<Guid> roleUuids);
    }
}
