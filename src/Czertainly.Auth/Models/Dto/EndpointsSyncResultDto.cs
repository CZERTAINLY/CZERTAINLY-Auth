using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record EndpointsSyncResultDto
    {
        public List<EndpointDto> AddedEndpoints { get; init; } = new List<EndpointDto>();
        public List<EndpointDto> UpdatedEndpoints { get; init; } = new List<EndpointDto>();

    }
}
