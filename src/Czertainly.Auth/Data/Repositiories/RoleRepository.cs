using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Czertainly.Auth.Data.Repositiories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AuthDbContext repositoryContext) : base(repositoryContext, null, new[] { "Users" })
        {
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userUuid)
        {
            return await _dbSet.Include(r => r.Users).Where(r => r.Users.Any(u => u.Uuid == userUuid)).ToListAsync();
        }
    }
}
