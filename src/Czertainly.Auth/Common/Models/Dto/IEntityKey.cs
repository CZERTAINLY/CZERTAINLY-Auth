namespace Czertainly.Auth.Common.Models.Dto
{
    public interface IEntityKey
    {
        public long? Id { get; }

        public Guid? Uuid { get; }
    }
}
