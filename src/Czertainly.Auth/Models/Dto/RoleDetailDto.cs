using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record RoleDetailDto : RoleDto
    {
        [Required]
        public List<PermissionDto> Permissions { get; init; } = new List<PermissionDto>();

    }
}
