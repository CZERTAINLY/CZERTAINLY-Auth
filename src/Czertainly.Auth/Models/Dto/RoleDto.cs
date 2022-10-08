using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record RoleDto : CrudTimestampedResponseDto
    {
        [Required]
        public string Name { get; init; }

        public string? Description { get; init; }

        [Required]
        public bool SystemRole { get; init; }

    }
}
