using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Czertainly.Auth.Data.Repositiories
{
    public class EndpointRepository : BaseRepository<Models.Entities.Endpoint>, IEndpointRepository
    {
        public EndpointRepository(AuthDbContext repositoryContext) : base(repositoryContext, new[] { "Resource", "Action" }, new[] { "Resource", "Action" })
        {
        }

        public async Task<Dictionary<string, Models.Entities.Endpoint>> GetExistingEndpointsMap()
        {
            return await _dbSet.Include(e => e.Resource).Include(e => e.Action).ToDictionaryAsync(e => $"{e.Method} {e.RouteTemplate}");
        }
    }
}
