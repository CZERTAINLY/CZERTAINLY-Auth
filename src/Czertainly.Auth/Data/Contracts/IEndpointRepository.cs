using Czertainly.Auth.Common.Data.Repositories;

namespace Czertainly.Auth.Data.Contracts
{
    public interface IEndpointRepository : IBaseRepository<Models.Entities.Endpoint>
    {
        Task<Dictionary<string, Models.Entities.Endpoint>> GetExistingEndpointsMap();

    }
}
