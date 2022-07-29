using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record PermissionDetailDto : PermissionDto
    {
        public RoleDto Role { get; init; }

    }
}
