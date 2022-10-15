using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record ActionRequestDto : ICrudRequestDto
    {
        [Required]
        public string? Name { get; init; }

        public string? DisplayName { get; init; }

    }
}
