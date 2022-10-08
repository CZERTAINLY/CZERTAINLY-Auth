using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Czertainly.Auth.Common.Models.Dto
{
    public record CrudTimestampedResponseDto : CrudResponseDto
    {
        [JsonPropertyOrder(99)]
        public DateTimeOffset CreatedAt { get; init; }

        [JsonPropertyOrder(100)]
        public DateTimeOffset UpdatedAt { get; init; }
    }
}
