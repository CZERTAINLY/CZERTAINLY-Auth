using System.Text.Json.Serialization;

namespace Czertainly.Auth.Common.Models.Dto
{
    public record QueryRequestDto : IQueryRequestDto
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }
    }
}
