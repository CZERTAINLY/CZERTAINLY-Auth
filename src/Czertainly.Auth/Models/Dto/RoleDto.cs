using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record RoleDto : IResourceDto
    {
        public Guid Uuid { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }

    }
}
