using Czertainly.Auth.Common.Models.Dto;

namespace Czertainly.Auth.Models.Dto
{
    public record PermissionDto : CrudResponseDto
    {
        public ResourceDto Resource { get; init; }
        public ActionDto Action { get; init; }
        public Guid? ObjectUuid { get; init; }
        public bool IsAllowed { get; init; }


    }
}
