using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;

namespace Czertainly.Auth.Data.Repositiories
{
    public class ActionRepository : BaseRepository<Models.Entities.Action>, IActionRepository
    {
        public ActionRepository(AuthDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
