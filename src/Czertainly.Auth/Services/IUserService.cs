using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IUserService
    {
        Task<PagedResponse<UserDto>> GetUsersAsync();
    }
}
