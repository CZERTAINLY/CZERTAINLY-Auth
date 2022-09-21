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

        public async Task<Models.Entities.Action?> GetActionByNameAsync(string actionName)
        {
            return await _dbSet.Where(a => a.Name.Equals(actionName, StringComparison.Ordinal)).FirstOrDefaultAsync();
        }
    }
}
