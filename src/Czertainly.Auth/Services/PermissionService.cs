using AutoMapper;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Services
{
    public class PermissionService : CrudService<Permission, PermissionDto, PermissionDetailDto>, IPermissionService
    {

        public PermissionService(IRepositoryManager repositoryManager, IMapper mapper): base(repositoryManager, repositoryManager.Permission, mapper)
        {
        }
    }
}
