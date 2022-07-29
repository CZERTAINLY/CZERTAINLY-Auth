using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record EndpointDto : CrudResponseDto
    {
        public string Name { get; init; }
        public string Method { get; init; }
        public string RouteTemplate { get; init; }
        public string ResourceName { get; init; }
        public string ActionName { get; init; }

    }
}
