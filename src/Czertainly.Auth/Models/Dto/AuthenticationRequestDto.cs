using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record AuthenticationRequestDto
    {
        public string? CertificateContent { get; init; }

        public string? AuthenticationToken { get; init; }
        
        public string? SystemUsername { get; init; }

    }
}
