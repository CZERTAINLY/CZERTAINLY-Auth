using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record UserAuthInfoDto : IResourceDto
    {
        public Guid Uuid { get; init; }
        public string? CertificateUuid { get; init; }
        public string? CertificateSerialNumber { get; init; }
        public string? CertificateFingerprint { get; init; }
    }
}
