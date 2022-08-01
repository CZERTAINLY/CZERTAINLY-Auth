using Czertainly.Auth.Common.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Models.Entities;

[Table("permission")]
public class Permission : BaseEntity
{
    [Required]
    [Column("role_id")]
    public long RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; }

    [Column("resource_id")]
    public long? ResourceId { get; set; }

    [ForeignKey(nameof(ResourceId))]
    public Resource? Resource { get; set; }

    [Column("action_id")]
    public long? ActionId { get; set; }

    [ForeignKey(nameof(ActionId))]
    public Action? Action { get; set; }

    [Column("object_uuid")]
    public Guid? ObjectUuid { get; set; }

    [Required]
    [Column("is_allowed")]
    public bool IsAllowed { get; set; } = true;

}
