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

    [Column("service_name")]
    public string? ServiceName { get; set; }

    [Column("resource_name")]
    public string? ResourceName { get; set; }

    [Column("action_name")]
    public string? ActionName { get; set; }

    public ICollection<Permission> Permissions { get; set; }

}
