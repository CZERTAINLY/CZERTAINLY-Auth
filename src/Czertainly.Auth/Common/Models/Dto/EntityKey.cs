namespace Czertainly.Auth.Common.Models.Dto
{
    public record EntityKey : IEntityKey
    {
        public long? Id { get; init; }

        public Guid? Uuid { get; init; }
    }
}
