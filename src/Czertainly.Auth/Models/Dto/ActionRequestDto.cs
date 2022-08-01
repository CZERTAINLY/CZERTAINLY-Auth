using Czertainly.Auth.Common.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Czertainly.Auth.Models.Dto
{
    public record ActionRequestDto : ICrudRequestDto
    {
        [Required]
        public string Name { get; init; }

        [JsonIgnore]
        public long ResourceId{ get; init; }

        [Required]
        public string ResourceName { get; init; }

    }
}
