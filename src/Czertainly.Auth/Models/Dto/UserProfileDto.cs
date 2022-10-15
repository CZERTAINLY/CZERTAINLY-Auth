using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record UserProfileDto
    {
        [Required]
        public UserDto User { get; init; }

        [Required]
        public List<string> Roles { get; init; } = new List<string>();

        [Required]
        public SubjectPermissionsDto Permissions { get; init; }
    }
}
