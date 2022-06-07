using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record UserDto : IResourceDto
    {
        public long Id { get; init; }
        public Guid Uuid { get; init; }
        public string Username { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string Email { get; init; }
        public bool Enabled { get; init; }
    }
}
