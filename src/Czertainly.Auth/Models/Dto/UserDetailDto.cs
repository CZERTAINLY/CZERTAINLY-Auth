using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record UserDetailDto : UserDto
    {
        public List<RoleDto> Roles { get; init; } = new List<RoleDto>();

    }
}
