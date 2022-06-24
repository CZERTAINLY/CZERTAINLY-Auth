using Czertainly.Auth.Common.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czertainly.Auth.Models.Entities;

[Table("user_auth_info")]
public class UserAuthInfo : BaseEntity
{
    [Column("certificate_uuid")]
    public string? CertificateUuid { get; set; }

    [Column("certificate_serial_number")]
    public string? CertificateSerialNumber { get; set; }

    [Column("certificate_fingerprint")]
    public string? CertificateFingerprint { get; set; }

    [Required]
    [Column("user_id")]
    public long UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

}
