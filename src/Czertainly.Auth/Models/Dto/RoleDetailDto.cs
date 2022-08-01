using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record RoleDetailDto : RoleDto
    {
        public List<PermissionDto> Permissions { get; init; } = new List<PermissionDto>();

    }
}
