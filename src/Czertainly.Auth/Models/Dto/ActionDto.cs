using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record ActionDto : CrudResponseDto
    {
        public string Name { get; init; }
        public ResourceDto Resource { get; init; }

    }
}
