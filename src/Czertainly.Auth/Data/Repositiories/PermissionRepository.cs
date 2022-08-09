using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Czertainly.Auth.Data.Repositiories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AuthDbContext repositoryContext) : base(repositoryContext)
        {

        }

        public async Task<List<Permission>> GetUserPermissions(Guid userUuid)
        {
            return _dbSet.Include(p => p.Resource).Include(p => p.Action).Include(p => p.Role).ThenInclude(r => r.Users.Where(u => u.Uuid == userUuid)).OrderBy(p => p.ResourceId).ThenBy(p => p.ActionId).ThenBy(p => p.ObjectUuid).ToList();
        }
    }
}
