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
    [Column("resource_id")]
    public long ResourceId { get; set; }

    [ForeignKey(nameof(ResourceId))]
    public Resource Resource { get; set; }

    public ICollection<Endpoint> Endpoints { get; set; }
    public ICollection<Permission> Permissions { get; set; }
}
