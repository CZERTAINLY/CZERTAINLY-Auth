using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Models.Dto;

namespace Czertainly.Auth.Services
{
    public interface IPermissionService
    {
        Task<SubjectPermissionsDto> GetRolePermissionsAsync(Guid roleUuid);

        Task<ResourcePermissionsDto> GetRoleResourcesPermissionsAsync(Guid roleUuid, Guid resourceUuid);

        Task<SubjectPermissionsDto> SaveRolePermissionsAsync(Guid roleUuid, SubjectPermissionsDto rolePermissions);


        Task<List<ObjectPermissionsDto>> GetRoleObjectsPermissionsAsync(Guid roleUuid, Guid resourceUuid);

        Task SaveRoleObjectsPermissionsAsync(Guid roleUuid, Guid resourceUuid, List<ObjectPermissionsDto> objectsPermissions);

        Task SaveRoleObjectPermissionsAsync(Guid roleUuid, Guid resourceUuid, Guid objectUuid, ObjectPermissionsDto objectPermissions);

        Task DeleteRoleObjectPermissionsAsync(Guid roleUuid, Guid resourceUuid, Guid objectUuid);

        Task<SubjectPermissionsDto> GetUserPermissionsAsync(Guid userUuid);
    }
}
