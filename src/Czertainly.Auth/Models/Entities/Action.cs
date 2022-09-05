using Czertainly.Auth.Common.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Models.Entities;

[Table("action")]
public class Action : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("resource_uuid")]
    public Guid ResourceUuid { get; set; }

    [ForeignKey(nameof(ResourceUuid))]
    public Resource Resource { get; set; }

    //public ICollection<Permission> Permissions { get; set; }
}
