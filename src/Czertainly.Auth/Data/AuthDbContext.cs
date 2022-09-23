using Czertainly.Auth.Models.Entities;
using Czertainly.Auth.Models.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Czertainly.Auth.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Models.Entities.Action> Actions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("auth");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);

            var adminRoleUuid = new Guid("da5668e2-9d94-4375-98c4-d665083edceb");
            var adminUserUuid = new Guid("64050556-dce6-42f8-81b6-96e521dd64d7");
            var superadminRoleUuid = new Guid("d34f960b-75c9-4184-ba97-665d30a9ee8a");
            var superadminUserUuid = new Guid("967679bd-0b75-41eb-8e9e-fef1a5ba4aa6");

            modelBuilder.Entity<Role>().HasData(
                new Role { Uuid = superadminRoleUuid, Name = "Superadmin", SystemRole = true, Description = "Internal Czertianly system role with all permissions" },
                new Role { Uuid = adminRoleUuid, Name = "Admin", SystemRole = true, Description = "Internal Czertianly system role with all administrating permissions" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Uuid = superadminUserUuid, Username = "superadmin", Enabled = true, SystemUser = true, CertificateFingerprint = "e1481e7eb80a265189da1c42c21066b006ed46afc1b55dd610a31bb8ec5da8b8" },
                new User { Uuid = adminUserUuid, Username = "admin", Enabled = true, SystemUser = true }
            );

            modelBuilder.Entity("user_role").HasData(
                new { role_uuid = superadminRoleUuid, user_uuid = superadminUserUuid },
                new { role_uuid = adminRoleUuid, user_uuid = adminUserUuid }
            );

            var superadminPermissionUuid = new Guid("3053b9c9-239d-4717-9d23-97e01177a40b");
            modelBuilder.Entity<Permission>().HasData(new Permission { Uuid = superadminPermissionUuid, RoleUuid = superadminRoleUuid, IsAllowed = true });
        }
    }
}
