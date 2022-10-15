using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Czertainly.Auth.Data.Repositiories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AuthDbContext repositoryContext) : base(repositoryContext, null, new[] { "Roles" })
        {
        }

        public async Task<IEnumerable<User>> GetRoleUsersAsync(Guid roleUuid)
        {
            return await _dbSet.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.Uuid == roleUuid)).ToListAsync();
        }
    }
}
