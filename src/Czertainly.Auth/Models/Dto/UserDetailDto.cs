using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record UserDetailDto : UserDto
    {
        [Required]
        public List<RoleDto> Roles { get; init; } = new List<RoleDto>();

    }
}
