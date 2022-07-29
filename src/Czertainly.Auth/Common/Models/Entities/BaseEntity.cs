using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Common.Models.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Column("id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("uuid", Order = 1)]
        public Guid Uuid { get; set; } = Guid.NewGuid();
    }
}
