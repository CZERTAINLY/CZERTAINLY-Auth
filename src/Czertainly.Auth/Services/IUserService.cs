using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IUserService : ICrudService<UserDto, UserDetailDto>
    {
        Task<UserProfileDto> GetUserProfileAsync(string certificate);
        Task<UserDetailDto> AssignRoleAsync(IEntityKey userKey, IEntityKey roleKey);
        Task<UserDetailDto> AssignRolesAsync(IEntityKey userKey, IEnumerable<Guid> roleUuids);
    }
}
