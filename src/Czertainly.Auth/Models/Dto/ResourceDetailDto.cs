using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record ResourceDetailDto : ResourceDto
    {
        public List<ActionDto> Actions { get; init; } = new List<ActionDto>();

    }
}
