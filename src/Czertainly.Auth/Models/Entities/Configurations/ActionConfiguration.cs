using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Czertainly.Auth.Models.Entities.Configurations
{
    public class ActionConfiguration : IEntityTypeConfiguration<Action>
    {
        public void Configure(EntityTypeBuilder<Action> builder)
        {
        }
    }
}
