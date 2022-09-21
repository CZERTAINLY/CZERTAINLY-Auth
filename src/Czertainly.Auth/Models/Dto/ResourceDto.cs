using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record ResourceDto : CrudResponseDto
    {
        [Required]
        public string Name { get; init; }
        
        [Required]
        public string DisplayName { get; init; }

        [Required]
        public bool ObjectAccess { get { return !string.IsNullOrEmpty(ListingEndpoint); } }
        
        public string? ListingEndpoint { get; init; }

    }
}
