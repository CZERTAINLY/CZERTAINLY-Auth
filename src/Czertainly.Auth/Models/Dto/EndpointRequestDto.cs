using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record EndpointRequestDto : ICrudRequestDto
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public string Method { get; init; }
        [Required]
        public string RouteTemplate { get; init; }
        [Required]
        public string ResourceName { get; init; }
        [Required]
        public string ActionName { get; init; }
        public bool IsListingEndpoint { get; init; } = false;
    }
}
