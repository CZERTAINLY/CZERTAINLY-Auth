using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Czertainly.Auth.Models.Dto
{
    public record RoleUpdateRequestDto : ICrudRequestDto
    {
        public string? Description { get; init; }

    }
}
