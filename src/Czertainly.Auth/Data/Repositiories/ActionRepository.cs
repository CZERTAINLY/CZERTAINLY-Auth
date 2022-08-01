using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Czertainly.Auth.Data.Repositiories
{
    public class ActionRepository : BaseRepository<Models.Entities.Action>, IActionRepository
    {
        public ActionRepository(AuthDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Models.Entities.Action?> GetResourceActionAsync(string actionName, string resourceName)
        {
            return await _dbSet.Include(a => a.Resource).Where(a => a.Name.Equals(actionName, StringComparison.Ordinal) && a.Resource.Name.Equals(resourceName, StringComparison.Ordinal)).FirstOrDefaultAsync();
        }
    }
}
