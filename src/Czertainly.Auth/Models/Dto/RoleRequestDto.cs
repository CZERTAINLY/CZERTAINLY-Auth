using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record RoleRequestDto : IRequestDto
    {
        public string Name { get; init; }
        public string? Description { get; init; }

    }
}
