using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IResourceService : ICrudService<ResourceDto, ResourceDetailDto>
    {
        Task<List<ResourceDetailDto>> GetAllResourcesAsync();

        Task<SyncResourcesResponseDto> SyncResourcesAsync(List<ResourceSyncRequestDto> resources);

    }
}
