using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record UserCertificateDto
    {
        public Guid? Uuid { get; init; }
        public string? Fingerprint { get; init; }
    }
}
