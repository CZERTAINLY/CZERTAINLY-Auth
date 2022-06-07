using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Czertainly.Auth.Data.Repositiories
{
    public class EndpointRepository : BaseRepository<Models.Entities.Endpoint>, IEndpointRepository
    {
        public EndpointRepository(AuthDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
