namespace Czertainly.Auth.Common.Data
{
    public interface IEntityKey
    {
        public long? Id { get; }

        public Guid? Uuid { get; }
    }
}
