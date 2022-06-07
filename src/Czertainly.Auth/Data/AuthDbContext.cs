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
        public DbSet<Models.Entities.Endpoint> Endpoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("auth");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }
    }
}
