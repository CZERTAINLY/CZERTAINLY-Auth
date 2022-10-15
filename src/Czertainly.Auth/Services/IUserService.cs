using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IUserService : ICrudService<UserDto, UserDetailDto>
    {
        Task<AuthenticationResponseDto> AuthenticateUserAsync(AuthenticationRequestDto authenticationRequestDto);

        Task<UserDetailDto> EnableUserAsync(Guid userUuid, bool enableFlag);

        Task<UserDetailDto> AssignRoleAsync(Guid userUuid, Guid roleUuid);
        Task<UserDetailDto> AssignRolesAsync(Guid userUuid, IEnumerable<Guid> roleUuids);
        Task<UserDetailDto> RemoveRoleAsync(Guid userUuid, Guid roleUuid);

        Task<List<UserDto>> GetRoleUsersAsync(Guid roleUuid);

    }
}
