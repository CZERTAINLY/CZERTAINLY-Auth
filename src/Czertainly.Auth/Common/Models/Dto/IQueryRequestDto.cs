namespace Czertainly.Auth.Common.Models.Dto
{
    public interface IQueryRequestDto
    {
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }

        public string SortBy { get; set; }
        public string SortDirection { get; set; }
    }
}
