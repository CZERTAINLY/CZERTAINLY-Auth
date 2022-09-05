using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record RoleDto : CrudResponseDto
    {
        public string Name { get; init; }
        public string? Description { get; init; }

        public bool SystemRole { get; init; }

    }
}
