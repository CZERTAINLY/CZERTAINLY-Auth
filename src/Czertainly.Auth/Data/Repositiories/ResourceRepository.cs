using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Czertainly.Auth.Data.Repositiories
{
    public class ResourceRepository : BaseRepository<Resource>, IResourceRepository
    {
        public ResourceRepository(AuthDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<List<Resource>> GetResourcesWithActions()
        {
            return await _dbSet.Include(r => r.Actions).ToListAsync();
        }

        public async Task<Dictionary<TKey, Resource>> GetResourcesMap<TKey>(Func<Resource, TKey> keySelector) where TKey : notnull
        {
            return await _dbSet.Include(r => r.Actions).AsTracking().ToDictionaryAsync(keySelector);
        }
    }
}
