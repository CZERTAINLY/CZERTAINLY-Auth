using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record RolePermissionsRequestDto
    {
        [Required]
        public bool AllowAllResources { get; set; }

        public List<ResourcePermissionsRequestDto>? Resources { get; init; }
    }
}