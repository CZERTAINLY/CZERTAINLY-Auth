using AutoMapper;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Services
{
    public class PermissionService : CrudService<Permission, PermissionDto, PermissionDetailDto>, IPermissionService
    {

        public PermissionService(IRepositoryManager repositoryManager, IMapper mapper): base(repositoryManager, repositoryManager.Permission, mapper)
        {
        }

        public async Task<MergedPermissionsDto> GetUserPermissionsAsync(Guid userUuid)
        {
            var result = new MergedPermissionsDto();

            var resourcesMapping = new SortedDictionary<string, ResourcePermissionsDto>(StringComparer.Ordinal);
            var resourceActionsMapping = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            var resourceObjectsMapping = new Dictionary<string, HashSet<Guid>>();
            var objectAllowedActionsMapping = new Dictionary<Guid, HashSet<string>>();
            var objectDeniedActionsMapping = new Dictionary<Guid, HashSet<string>>();

            var permissions = await _repositoryManager.Permission.GetUserPermissions(userUuid);
            foreach (var permission in permissions)
            {
                // if some role allows all resources and other no, allow all and let overriding on lower level
                if (!permission.ResourceId.HasValue)
                {
                    result.AllowAllResources = result.AllowAllResources || permission.IsAllowed;
                    continue;
                }

                var resourceName = permission.Resource.Name;
                if (!resourcesMapping.TryGetValue(resourceName, out var resource)) resourcesMapping.Add(resourceName, resource = new ResourcePermissionsDto { Name = resourceName });
                if(!permission.ActionId.HasValue)
                {
                    // if some role allows all resource actions and other no, allow all and let overriding on lower level
                    resource.AllowAllActions = resource.AllowAllActions || permission.IsAllowed;
                    continue;
                }

                var actionName = permission.Action.Name;
                if (!resourceActionsMapping.TryGetValue(resourceName, out var resourceActions)) resourceActionsMapping.Add(resourceName, resourceActions = new HashSet<string>(StringComparer.Ordinal));
                if (!permission.ObjectUuid.HasValue)
                {
                    // deny action on resource for all object not supported
                    if (!permission.IsAllowed) continue;

                    // add action to resource allowed actions
                    resourceActions.Add(actionName);
                    continue;
                }

                // add object UUID to resource object permissions
                var objectUuid = permission.ObjectUuid.Value;
                if (!resourceObjectsMapping.TryGetValue(resourceName, out var resourceObjects)) resourceObjectsMapping.Add(resourceName, resourceObjects = new HashSet<Guid>());
                resourceObjects.Add(objectUuid);

                // add action to object allow/deny actions
                var actionsMapping = permission.IsAllowed ? objectAllowedActionsMapping : objectDeniedActionsMapping;
                if (!actionsMapping.TryGetValue(objectUuid, out var objectActions)) actionsMapping.Add(objectUuid, objectActions = new HashSet<string>(StringComparer.Ordinal));
                objectActions.Add(actionName);
            }

            // merge permissions and add it to result DTO
            foreach (var resourcePermissionsDto in resourcesMapping.Values)
            {
                // if resource allows all actions, individual actions listing is redundant
                HashSet<string>? resourceActions = null;
                if (!resourcePermissionsDto.AllowAllActions && resourceActionsMapping.TryGetValue(resourcePermissionsDto.Name, out resourceActions))
                {
                    resourcePermissionsDto.Actions.AddRange(resourceActions);
                    resourcePermissionsDto.Actions.Sort();
                }

                // merge resource objects permissions if there are some defined
                if (resourceObjectsMapping.TryGetValue(resourcePermissionsDto.Name, out var resourceObjects))
                {
                    foreach (var objectUuid in resourceObjects)
                    {
                        var objectPermissionsDto = new ObjectPermissionsDto { Uuid = objectUuid };

                        // first add denied actions
                        if (objectDeniedActionsMapping.TryGetValue(objectUuid, out var deniedActions))
                        {
                            objectPermissionsDto.Deny.AddRange(deniedActions);
                            objectPermissionsDto.Deny.Sort();
                        }

                        // then filter allowed actions
                        if (!resourcePermissionsDto.AllowAllActions && objectAllowedActionsMapping.TryGetValue(objectUuid, out var allowedActions))
                        {
                            foreach (var allowedAction in allowedActions)
                            {
                                // if not already allowed in resource actions or not already denied, add allowed action
                                if ((resourceActions == null || !resourceActions.Contains(allowedAction))
                                    && (deniedActions == null || !deniedActions.Contains(allowedAction))) objectPermissionsDto.Allow.Add(allowedAction);
                            }
                        }

                        resourcePermissionsDto.Objects.Add(objectPermissionsDto);
                    }
                }

                result.Resources.Add(resourcePermissionsDto);
            }

            return result;
        }
    }
}
