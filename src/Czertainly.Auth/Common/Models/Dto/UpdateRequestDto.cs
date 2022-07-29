using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Czertainly.Auth.Common.Models.Dto
{
    public record UpdateRequestDto : IUpdateRequestDto
    {
        [JsonIgnore]
        [HiddenInput(DisplayValue = false)]
        public long Id { get; init; }

        [FromRoute]
        [JsonIgnore]
        [HiddenInput(DisplayValue = false)]
        public Guid Uuid { get; init; }
    }
}
