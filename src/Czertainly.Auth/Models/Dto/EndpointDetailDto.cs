using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record EndpointDetailDto : CrudResponseDto
    {
        public string Name { get; init; }
        public string Method { get; init; }
        public string RouteTemplate { get; init; }
        public ResourceDto Resource { get; init; }
        public ActionDto Action { get; init; }
    }
}
