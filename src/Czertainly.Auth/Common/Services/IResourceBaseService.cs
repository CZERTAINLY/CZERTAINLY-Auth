using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Common.Services
{
    public interface IResourceBaseService <TResourceDto> where TResourceDto : IResourceDto, new()
    {
        public Task<PagedResponse<TResourceDto>> GetAsync(IQueryRequestDto dto);
        public Task<TResourceDto> CreateAsync(IRequestDto dto);
        public Task<TResourceDto> GetDetailAsync(IEntityKey key);
        public Task<TResourceDto> UpdateAsync(IEntityKey key, IRequestDto dto);
        public Task DeleteAsync(IEntityKey key);

    }
}
