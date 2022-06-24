using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Common.Models.Entities
{
    [Index(nameof(Uuid), IsUnique = true)]
    public abstract class BaseEntity : IBaseEntity
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("uuid")]
        public Guid Uuid { get; set; } = Guid.NewGuid();
    }
}
