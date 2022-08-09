using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record UserPermissionsDto
    {
        public bool AllowAllResources { get; init; }
        public List<ResourcePermissionsDto> Resources { get; init; } = new List<ResourcePermissionsDto>();
    }

    public record ResourcePermissionsDto
    {
        public string Name { get; init; }
        public bool AllowAllActions { get; init; }
        public List<string> Actions { get; init; } = new List<string>();
        public List<ObjectPermissionsDto> Objects { get; init; } = new List<ObjectPermissionsDto>();

    }

    public record ObjectPermissionsDto
    {
        public Guid Uuid { get; init; }
        public List<string> Allow { get; init; } = new List<string>();
        public List<string> Deny { get; init; } = new List<string>();

    }
}
