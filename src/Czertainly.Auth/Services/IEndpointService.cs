using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IEndpointService : ICrudService<EndpointDto, EndpointDetailDto>
    {
        public Task<SyncEndpointsResultDto> SyncEndpoints(List<EndpointRequestDto> endpoints);

    }
}
