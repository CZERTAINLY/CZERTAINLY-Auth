using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record EndpointDetailDto : CrudResponseDto
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Method { get; init; }

        [Required]
        public string RouteTemplate { get; init; }

        [Required]
        public ResourceDto Resource { get; init; }
        
        public ActionDto Action { get; init; }
    }
}
