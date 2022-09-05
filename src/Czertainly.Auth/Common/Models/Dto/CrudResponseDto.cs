using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Czertainly.Auth.Common.Models.Dto
{
    public record CrudResponseDto : ICrudResponseDto
    {
        [JsonPropertyOrder(-1)]
        public Guid Uuid { get; init;  }
    }
}
