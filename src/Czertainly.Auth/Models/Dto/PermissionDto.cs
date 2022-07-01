using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record PermissionDto : IResourceDto
    {
        public Guid Uuid { get; init; }
        public bool AllowAll { get; init; }
        public RoleDto Role { get; init; }
        public EndpointDto Endpoint { get; init; }

    }
}
