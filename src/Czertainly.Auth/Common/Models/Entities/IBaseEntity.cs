namespace Czertainly.Auth.Common.Models.Entities
{
    public interface IBaseEntity
    {
        public long Id { get; set; }

        public Guid Uuid { get; set; }
    }
}
