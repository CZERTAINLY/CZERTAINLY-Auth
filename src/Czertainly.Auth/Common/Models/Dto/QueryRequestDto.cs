namespace Czertainly.Auth.Common.Models.Dto
{
    public record QueryRequestDto : IQueryRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 11;
        public string SortBy { get; set; } = "id";
    }
}
