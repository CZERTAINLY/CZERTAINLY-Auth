using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Data.Repositiories
{
    public class ResourceRepository : BaseRepository<Resource>, IResourceRepository
    {
        public ResourceRepository(AuthDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
