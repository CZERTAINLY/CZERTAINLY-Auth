using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IUserService : ICrudService<UserDto, UserDetailDto>
    {
        Task<AuthenticationResponseDto> AuthenticateUserAsync(string certificate);
        Task<UserDetailDto> AssignRoleAsync(Guid userKey, Guid roleKey);
        Task<UserDetailDto> AssignRolesAsync(Guid userKey, IEnumerable<Guid> roleUuids);
    }
}
