using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IEndpointService : IResourceBaseService<EndpointDto>
    {
        public Task<EndpointsSyncResultDto> SyncEndpoints(List<EndpointRequestDto> endpoints);
    }
}
