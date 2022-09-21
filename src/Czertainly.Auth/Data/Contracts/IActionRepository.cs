using Czertainly.Auth.Common.Data.Repositories;

namespace Czertainly.Auth.Data.Contracts
{
    public interface IActionRepository : IBaseRepository<Models.Entities.Action>
    {
        Task<Models.Entities.Action?> GetActionByNameAsync(string actionName);
    }
}
