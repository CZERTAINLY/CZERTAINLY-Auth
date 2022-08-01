using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IRoleService : ICrudService<RoleDto, RoleDetailDto>
    {
    }
}
