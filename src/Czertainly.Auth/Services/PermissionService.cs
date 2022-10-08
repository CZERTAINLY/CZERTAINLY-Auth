using AutoMapper;
using Czertainly.Auth.Common.Exceptions;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Models.Entities;

namespace Czertainly.Auth.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _repository;
        private readonly IRepositoryManager _repositoryManager;

        public PermissionService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _repository = repositoryManager.Permission;
        }

        #region Retrieving permissions

        public async Task<SubjectPermissionsDto> GetRolePermissionsAsync(Guid roleUuid)
        {
            await CheckRole(roleUuid);

            var permissions = await _repository.GetRolePermissions(roleUuid);

            return MergePermissions(permissions);
        }

        public async Task<ResourcePermissionsDto> GetRoleResourcesPermissionsAsync(Guid roleUuid, Guid resourceUuid)
        {
            await CheckRole(roleUuid);

            var permissions = await _repository.GetRoleResourcePermissions(roleUuid, resourceUuid);
            var subjectPermissions = MergePermissions(permissions);

            if(subjectPermissions.Resources.Count != 0) return subjectPermissions.Resources[0];

            var resource = await _repositoryManager.Resource.GetByKeyAsync(resourceUuid);
            return new ResourcePermissionsDto { Name = resource.Name };
        }

        public async Task<List<ObjectPermissionsDto>> GetRoleObjectsPermissionsAsync(Guid roleUuid, Guid resourceUuid)
        {
            await CheckRole(roleUuid);

            var objectsMapping = new SortedDictionary<Guid, ObjectPermissionsDto>();
            var actionsMapping = await _repositoryManager.Action.GetDictionaryMap(a => a.Uuid);

            var permissions = await _repository.GetRoleResourceObjectsPermissions(roleUuid, resourceUuid);
            foreach (var permission in permissions)
            {
                if (!permission.ActionUuid.HasValue || !permission.ObjectUuid.HasValue) continue;
                if (!objectsMapping.TryGetValue(permission.ObjectUuid.Value, out var objectPermissions)) objectsMapping.Add(permission.ObjectUuid.Value, objectPermissions = new ObjectPermissionsDto { Uuid = permission.ObjectUuid.Value });

                var actionName = actionsMapping[permission.ActionUuid.Value].Name;
                if (permission.IsAllowed) objectPermissions.Allow.Add(actionName);
                else objectPermissions.Deny.Add(actionName);
            }

            return objectsMapping.Values.ToList();
        }

        public async Task<SubjectPermissionsDto> GetUserPermissionsAsync(Guid userUuid)
        {
            var user = await _repositoryManager.User.GetByKeyAsync(userUuid);

            var permissions = await _repository.GetUserPermissions(userUuid);

            return MergePermissions(permissions);
        }

        #endregion

        #region Updating permissions

        public async Task<SubjectPermissionsDto> SaveRolePermissionsAsync(Guid roleUuid, RolePermissionsRequestDto rolePermissions)
        {
            await CheckRole(roleUuid, true);

            _repository.DeleteRolePermissionsWithoutObjects(roleUuid);

            var resourcesMapping = await _repositoryManager.Resource.GetDictionaryMap(r => r.Name);
            var actionsMapping = await _repositoryManager.Action.GetDictionaryMap(a => a.Name);

            if (rolePermissions.AllowAllResources) _repository.Create(new Permission { RoleUuid = roleUuid, IsAllowed = true });
            if (rolePermissions.Resources != null)
            {
                foreach (var resourcePermissions in rolePermissions.Resources)
                {
                    if (!resourcesMapping.TryGetValue(resourcePermissions.Name, out var resource)) throw new EntityNotFoundException($"Unknown resource '{resourcePermissions.Name}'");

                    var resourceUuid = resource.Uuid;
                    if (resourcePermissions.AllowAllActions) _repository.Create(new Permission { RoleUuid = roleUuid, ResourceUuid = resourceUuid, IsAllowed = true });
                    else if(resourcePermissions.Actions != null)
                    {
                        foreach (var actionName in resourcePermissions.Actions)
                        {
                            if (!actionsMapping.TryGetValue(actionName, out var action)) throw new EntityNotFoundException($"Unknown action '{actionName}'");

                            var actionUuid = action.Uuid;
                            _repository.Create(new Permission { RoleUuid = roleUuid, ResourceUuid = resourceUuid, ActionUuid = actionUuid, IsAllowed = true });
                        }
                    }

                    if (resourcePermissions.Objects != null)
                    {
                        _repository.DeleteRoleResourceObjectsPermissions(roleUuid, resourceUuid);
                        if (resourcePermissions.Objects.Count > 0 && string.IsNullOrEmpty(resource.ListObjectsEndpoint)) throw new InvalidActionException($"Cannot save object permissions. Resource '{resource.DisplayName}' does not support object access permissions");
                        var resourceActions = resourcePermissions.AllowAllActions ? null : (resourcePermissions.Actions ?? new List<string>());
                        foreach (var objectPermissions in resourcePermissions.Objects)
                        {
                            AddRoleResourceObjectPermissions(roleUuid, resourceUuid, objectPermissions, resourceActions, actionsMapping);
                        }
                    }
                }
            }

            await _repositoryManager.SaveAsync();

            return await GetRolePermissionsAsync(roleUuid);
        }

        public async Task SaveRoleObjectsPermissionsAsync(Guid roleUuid, Guid resourceUuid, List<ObjectPermissionsRequestDto> objectsPermissions)
        {
            await CheckRole(roleUuid, true);

            var resource = await _repositoryManager.Resource.GetByKeyAsync(resourceUuid);
            if (String.IsNullOrEmpty(resource.ListObjectsEndpoint)) throw new InvalidActionException($"Cannot save object permissions. Resource '{resource.DisplayName}' does not support object access permissions");

            _repository.DeleteRoleResourceObjectsPermissions(roleUuid, resourceUuid);

            var actionsMapping = await _repositoryManager.Action.GetDictionaryMap(a => a.Name);
            var resourcePermissions = await GetRoleResourcesPermissionsAsync(roleUuid, resourceUuid);
            foreach (var objectPermissions in objectsPermissions)
            {
                AddRoleResourceObjectPermissions(roleUuid, resourceUuid, objectPermissions, resourcePermissions.AllowAllActions ? null : resourcePermissions.Actions, actionsMapping);
            }

            await _repositoryManager.SaveAsync();
        }

        public async Task SaveRoleObjectPermissionsAsync(Guid roleUuid, Guid resourceUuid, Guid objectUuid, ObjectPermissionsRequestDto objectPermissions)
        {
            await CheckRole(roleUuid, true);

            var resource = await _repositoryManager.Resource.GetByKeyAsync(resourceUuid);
            if (String.IsNullOrEmpty(resource.ListObjectsEndpoint)) throw new InvalidActionException($"Cannot save object permissions. Resource '{resource.DisplayName}' does not support object access permissions");

            _repository.DeleteRoleResourceObjectPermissions(roleUuid, resourceUuid, objectUuid);
            var actionsMapping = await _repositoryManager.Action.GetDictionaryMap(a => a.Name);
            var resourcePermissions = await GetRoleResourcesPermissionsAsync(roleUuid, resourceUuid);
            AddRoleResourceObjectPermissions(roleUuid, resourceUuid, objectPermissions, resourcePermissions.AllowAllActions ? null : resourcePermissions.Actions, actionsMapping);

            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteRoleObjectPermissionsAsync(Guid roleUuid, Guid resourceUuid, Guid objectUuid)
        {
            await CheckRole(roleUuid, true);

            _repository.DeleteRoleResourceObjectPermissions(roleUuid, resourceUuid, objectUuid);

            await _repositoryManager.SaveAsync();
        }

        #endregion

        private async Task CheckRole(Guid roleUuid, bool update = false)
        {
            var role = await _repositoryManager.Role.GetByKeyAsync(roleUuid);
            if (update && role.SystemRole) throw new InvalidActionException("Cannot update system role permissions");
        }

        private void AddRoleResourceObjectPermissions(Guid roleUuid, Guid resourceUuid, ObjectPermissionsRequestDto objectPermissions, List<string>? resourceActions, Dictionary<string, Models.Entities.Action> actionsMapping)
        {
            var allowActions = resourceActions == null || objectPermissions.Allow == null ? Enumerable.Empty<string>() : objectPermissions.Allow.Where(a => !resourceActions.Contains(a));
            foreach (var allowAction in allowActions)
            {
                if (!actionsMapping.TryGetValue(allowAction, out var action)) throw new EntityNotFoundException($"Unknown action '{allowAction}'");
                var actionUuid = action.Uuid;

                _repository.Create(new Permission
                {
                    RoleUuid = roleUuid,
                    ResourceUuid = resourceUuid,
                    ActionUuid = actionUuid,
                    ObjectUuid = objectPermissions.Uuid,
                    IsAllowed = true
                });
            }

            if (objectPermissions.Deny != null)
            {
                foreach (var denyAction in objectPermissions.Deny)
                {
                    if (!actionsMapping.TryGetValue(denyAction, out var action)) throw new EntityNotFoundException($"Unknown action '{denyAction}'");
                    var actionUuid = action.Uuid;

                    _repository.Create(new Permission
                    {
                        RoleUuid = roleUuid,
                        ResourceUuid = resourceUuid,
                        ActionUuid = actionUuid,
                        ObjectUuid = objectPermissions.Uuid,
                        IsAllowed = false
                    });
                }
            }
        }

        private SubjectPermissionsDto MergePermissions(IEnumerable<Permission> permissions)
        {
            var result = new SubjectPermissionsDto();

            var resourcesMapping = new SortedDictionary<string, ResourcePermissionsDto>(StringComparer.Ordinal);
            var resourceActionsMapping = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            var resourceObjectsMapping = new Dictionary<string, HashSet<Guid>>();
            var objectAllowedActionsMapping = new Dictionary<Guid, HashSet<string>>();
            var objectDeniedActionsMapping = new Dictionary<Guid, HashSet<string>>();

            foreach (var permission in permissions)
            {
                // if some role allows all resources and other no, allow all and let overriding on lower level
                if (!permission.ResourceUuid.HasValue)
                {
                    result.AllowAllResources = result.AllowAllResources || permission.IsAllowed;
                    continue;
                }

                var resourceName = permission.Resource.Name;
                if (!resourcesMapping.TryGetValue(resourceName, out var resource)) resourcesMapping.Add(resourceName, resource = new ResourcePermissionsDto { Name = resourceName });
                if (!permission.ActionUuid.HasValue)
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
