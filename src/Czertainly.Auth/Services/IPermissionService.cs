using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IPermissionService : ICrudService<PermissionDto, PermissionDetailDto>
    {
    }
}
