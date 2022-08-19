using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record UserProfileDto
    {
        public UserDto User { get; init; }
        public List<string> Roles { get; init; } = new List<string>();
        public MergedPermissionsDto Permissions { get; init; }
    }
}
