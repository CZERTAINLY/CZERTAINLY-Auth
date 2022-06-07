namespace Czertainly.Auth.Common.Models.Dto
{
    public interface IResourceDto
    {
        public long Id { get; init; }

        public Guid Uuid { get; init; }
    }
}
