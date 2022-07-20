using Czertainly.Auth.Common.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Models.Entities;

[Table("resource")]
public class Resource : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("listing_endpoint")]
    public string? ListingEndpoint { get; set; }

    public ICollection<Action> Actions { get; set; }

    public ICollection<Endpoint> Endpoints { get; set; }

    public ICollection<Permission> Permissions { get; set; }
}
