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
    public Role Role{ get; set; }

    [Required]
    [Column("endpoint_id")]
    public long EndpointId { get; set; }

    [ForeignKey(nameof(EndpointId))]
    public Endpoint Endpoint { get; set; }

    public bool AllowAll { get; set; } = true;

}
