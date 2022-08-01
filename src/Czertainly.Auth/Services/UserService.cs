using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Services
{
    public class UserService : CrudService<User, UserDto, UserDetailDto>, IUserService
    {
        public UserService(IRepositoryManager repositoryManager, IMapper mapper): base(repositoryManager, repositoryManager.User, mapper)
        {
            
        }

        public async Task<UserProfileDto> GetUserProfileAsync(UserProfileRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDetailDto> AssignRoleAsync(IEntityKey userKey, IEntityKey roleKey)
        {
            var user = await _repository.GetByKeyAsync(userKey);
            var role = await _repositoryManager.Role.GetByKeyAsync(roleKey);

            user.Roles.Add(role);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<UserDetailDto>(user);
        }

        public async Task<UserDetailDto> AssignRolesAsync(IEntityKey userKey, IEnumerable<Guid> roleUuids)
        {
            var user = await _repository.GetByKeyAsync(userKey);
            var roles = await _repositoryManager.Role.GetByUuidsAsync(roleUuids);

            user.Roles.Clear();
            foreach (var role in roles) user.Roles.Add(role);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<UserDetailDto>(user);
        }
    }
}
