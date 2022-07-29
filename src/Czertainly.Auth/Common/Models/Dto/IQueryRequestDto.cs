namespace Czertainly.Auth.Common.Models.Dto
{
    public interface IQueryRequestDto
    {
        public int PageNumber { get; }
        public int ItemsPerPage { get; }
        public string SortBy { get; }
    }
}
