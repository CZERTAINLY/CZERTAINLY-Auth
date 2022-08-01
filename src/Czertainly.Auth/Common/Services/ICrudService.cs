using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Common.Services
{
    public interface ICrudService<TResponseDto, TDetailResponseDto>
        where TResponseDto : ICrudResponseDto, new()
        where TDetailResponseDto : ICrudResponseDto, new()
    {
        public Task<PagedResponse<TResponseDto>> GetAsync(IQueryRequestDto dto);
        public Task<TResponseDto> CreateAsync(ICrudRequestDto dto);
        public Task<TDetailResponseDto> GetDetailAsync(IEntityKey key);
        public Task<TResponseDto> UpdateAsync(IEntityKey key, ICrudRequestDto dto);
        public Task DeleteAsync(IEntityKey key);

    }
}
