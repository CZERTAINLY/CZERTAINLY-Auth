using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record RoleRequestDto : ICrudRequestDto
    {
        [Required]
        public string? Name { get; init; }

        public string? Description { get; init; }

        public bool? SystemRole { get; init; }

        public RolePermissionsRequestDto? Permissions { get; init; }

    }
}
