using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record SyncEndpointsResultDto
    {
        public SyncEndpointsDto Endpoints { get; init; } = new SyncEndpointsDto();
        public SyncResourcesDto Resources { get; init; } = new SyncResourcesDto();
        public SyncActionsDto Actions { get; init; } = new SyncActionsDto();

    }

    public record SyncEndpointsDto
    {
        public List<EndpointDto> Added { get; init; } = new List<EndpointDto>();
        public List<EndpointDto> Updated { get; init; } = new List<EndpointDto>();
        public List<EndpointDto> Removed { get; init; } = new List<EndpointDto>();

    }

    public record SyncResourcesDto
    {
        public List<ResourceDto> Added { get; init; } = new List<ResourceDto>();
        public List<ResourceDto> Unused { get; init; } = new List<ResourceDto>();

    }

    public record SyncActionsDto
    {
        public List<ActionDto> Added { get; init; } = new List<ActionDto>();
        public List<ActionDto> Unused { get; init; } = new List<ActionDto>();

    }
}
