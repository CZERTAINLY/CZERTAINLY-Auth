using Czertainly.Auth.Common.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Models.Entities;

[Table("role")]
[Index(nameof(Name), IsUnique = true)]
public class Role : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("system_role")]
    public bool SystemRole { get; set; } = false;

    public ICollection<User> Users { get; set; }

    public ICollection<Permission> Permissions { get; set; }
}
