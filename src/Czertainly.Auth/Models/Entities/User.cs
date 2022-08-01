using Czertainly.Auth.Common.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Models.Entities;

[Table("user")]
public class User : BaseEntity
{
    [Required]
    [Column("username")]
    public string Username { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Required]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("enabled")]
    public bool Enabled { get; set; } = true;

    [Column("certificate_uuid")]
    public Guid? CertificateUuid { get; set; }

    [Column("certificate_fingerprint")]
    public string? CertificateFingerprint { get; set; }

    public ICollection<Role> Roles { get; set; }

}
