using AutoMapper;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Services
{
    public class RoleService : CrudService<Role, RoleDto, RoleDetailDto>, IRoleService
    {

        public RoleService(IRepositoryManager repositoryManager, IMapper mapper): base(repositoryManager, repositoryManager.Role, mapper)
        {
        }
    }
}
