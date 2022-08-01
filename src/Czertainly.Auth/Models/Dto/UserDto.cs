using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record UserDto : CrudResponseDto
    {
        public string Username { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string Email { get; init; }
        public bool Enabled { get; init; }
        public UserCertificateDto? Certificate { get; init; }
    }
}
