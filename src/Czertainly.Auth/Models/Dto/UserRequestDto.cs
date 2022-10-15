using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record UserRequestDto : ICrudRequestDto
    {
        [Required]
        public string? Username { get; init; }
        
        public string? FirstName { get; init; }
        
        public string? LastName { get; init; }

        public bool? Enabled { get; init; }

        public bool? SystemUser { get; init; }

        [EmailAddress]
        public string? Email { get; init; }

        public string? Description { get; init; }

        public Guid? CertificateUuid { get; init; }

        public string? CertificateFingerprint { get; init; }

    }
}
