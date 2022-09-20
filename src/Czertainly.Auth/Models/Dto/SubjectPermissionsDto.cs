namespace Czertainly.Auth.Models.Dto
{
    public record SubjectPermissionsDto
    {
        public bool AllowAllResources { get; set; }

        public List<ResourcePermissionsDto> Resources { get; init; } = new List<ResourcePermissionsDto>();
    }
}