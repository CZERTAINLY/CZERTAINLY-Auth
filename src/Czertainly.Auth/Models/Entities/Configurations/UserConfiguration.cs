using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Czertainly.Auth.Models.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "user_role",
                    x => x.HasOne<Role>().WithMany().HasForeignKey("role_uuid").OnDelete(DeleteBehavior.Cascade),
                    x => x.HasOne<User>().WithMany().HasForeignKey("user_uuid").OnDelete(DeleteBehavior.Cascade));
        }
    }
}
