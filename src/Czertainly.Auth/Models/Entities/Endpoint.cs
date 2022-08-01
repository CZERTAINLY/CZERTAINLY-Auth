using Czertainly.Auth.Common.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Models.Entities;

[Table("endpoint")]
public class Endpoint : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("method")]
    public string Method { get; set; }

    [Required]
    [Column("route_template")]
    public string RouteTemplate { get; set; }

    [Required]
    [Column("resource_id")]
    public long ResourceId { get; set; }

    [ForeignKey(nameof(ResourceId))]
    public Resource Resource { get; set; }

    [Required]
    [Column("action_id")]
    public long ActionId { get; set; }

    [ForeignKey(nameof(ActionId))]
    public Action Action { get; set; }

}
