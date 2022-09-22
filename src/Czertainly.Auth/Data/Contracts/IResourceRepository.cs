using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Data.Contracts
{
    public interface IResourceRepository : IBaseRepository<Resource>
    {
        Task<List<Resource>> GetResourcesWithActions();

        Task<Dictionary<TKey, Resource>> GetResourcesMap<TKey>(Func<Resource, TKey> keySelector) where TKey : notnull;
    }
}
