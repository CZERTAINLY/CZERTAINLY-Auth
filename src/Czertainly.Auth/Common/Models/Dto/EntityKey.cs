using Czertainly.Auth.Common.Data;

namespace Czertainly.Auth.Common.Models.Dto
{
    public record EntityKey : IEntityKey
    {
        public long? Id { get; init; }

        public Guid? Uuid { get; init; }

        //public EntityKey()
        //{

        //}

        public EntityKey(long id)
        {
            Id = Id;
        }

        public EntityKey(Guid uuid)
        {
            Uuid = uuid;
        }

        public EntityKey(long id, Guid uuid)
        {
            Id = Id;
            Uuid = uuid;
        }
    }
}
