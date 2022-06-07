using AutoMapper;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public class UserService : IUserService, IResourceService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public UserService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }

        public async Task<PagedResponse<UserDto>> GetUsersAsync()
        {
            var users = await _repositoryManager.User.GetAllAsync(new QueryStringParameters());

            return new PagedResponse<UserDto>
            {
                Data = _mapper.Map<List<UserDto>>(users),
                Links = _mapper.Map<PagingMetadata>(users),
            };
        }
    }
}
