using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czertainly.Auth.Data.Contracts
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        IRoleRepository Role { get; }
        IPermissionRepository Permission { get; }
        IEndpointRepository Endpoint { get; }
        IResourceRepository Resource { get; }
        IActionRepository Action { get; }

        Task SaveAsync();
    }
}
