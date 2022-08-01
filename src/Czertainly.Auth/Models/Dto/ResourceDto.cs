using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record ResourceDto : CrudResponseDto
    {
        public string Name { get; init; }
        public string? ListingEndpoint { get; init; }

    }
}
