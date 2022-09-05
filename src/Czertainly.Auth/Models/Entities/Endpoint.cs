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
    [Column("resource_uuid")]
    public Guid ResourceUuid { get; set; }

    [ForeignKey(nameof(ResourceUuid))]
    public Resource Resource { get; set; }

    [Column("action_uuid")]
    public Guid? ActionUuid { get; set; }

    [ForeignKey(nameof(ActionUuid))]
    public Action? Action { get; set; }

}
