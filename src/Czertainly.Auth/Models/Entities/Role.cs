using Czertainly.Auth.Common.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Models.Entities;

[Table("role")]
public class Role : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    public ICollection<User> Users { get; set; }

    public ICollection<Permission> Permissions { get; set; }
}
