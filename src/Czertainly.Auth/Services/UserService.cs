using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Services
{
    public class UserService : ResourceService<User, UserDto>, IUserService
    {
        public UserService(IRepositoryManager repositoryManager, IMapper mapper): base(repositoryManager, repositoryManager.User, mapper)
        {
            
        }

        public Task<UserDto> AssignRole(IEntityKey userKey, IEntityKey roleKey)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> AssignRoles(IEntityKey userKey, List<Guid> roleUuids)
        {
            throw new NotImplementedException();
        }
    }
}
