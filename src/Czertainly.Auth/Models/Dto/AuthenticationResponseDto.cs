using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record AuthenticationResponseDto
    {
        public bool Authenticated { get; init; }
        public UserProfileDto? Data { get; init; }
    }
}
