using AutoMapper;
using Czertainly.Auth.Common.Exceptions;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Services
{
    public class RoleService : CrudService<Role, RoleDto, RoleDetailDto>, IRoleService
    {
        private readonly IPermissionService _permissionService;

        public RoleService(IRepositoryManager repositoryManager, IMapper mapper, IPermissionService permissionService) : base(repositoryManager, repositoryManager.Role, mapper)
        {
            _permissionService = permissionService;
        }

        public override async Task<RoleDetailDto> CreateAsync(ICrudRequestDto dto)
        {
            var roleRequestDto = dto as RoleRequestDto;
            if(roleRequestDto == null) throw new InvalidActionException("Cannot create role. Invalid DTO");

            // check uniqueness of role
            if (_repository.GetByConditionAsync(r => r.Name == roleRequestDto.Name) != null) throw new EntityNotUniqueException($"Role with name '{roleRequestDto.Name}' already exists");

            var newRole = await base.CreateAsync(dto);
            if (roleRequestDto.Permissions != null) await _permissionService.SaveRolePermissionsAsync(newRole.Uuid, roleRequestDto.Permissions);

            return newRole;
        }

        public override async Task<RoleDetailDto> UpdateAsync(Guid key, ICrudRequestDto dto)
        {
            var role = await _repository.GetByKeyAsync(key);
            if (role.SystemRole) throw new InvalidActionException("Cannot update system role.");

            return await base.UpdateAsync(key, dto);
        }

        public override async Task DeleteAsync(Guid key)
        {
            var role = await _repository.GetByKeyAsync(key);
            if (role.SystemRole) throw new InvalidActionException("Cannot delete system role.");

            await base.DeleteAsync(key);
        }

        public async Task<List<RoleDto>> GetUserRolesAsync(Guid userUuid)
        {
            var roles = await _repositoryManager.Role.GetUserRolesAsync(userUuid);
            return _mapper.Map<List<RoleDto>>(roles);
        }
    }
}
