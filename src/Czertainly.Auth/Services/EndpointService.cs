using AutoMapper;
using Czertainly.Auth.Common.Exceptions;
using Czertainly.Auth.Common.Helpers;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Endpoint = Czertainly.Auth.Models.Entities.Endpoint;

namespace Czertainly.Auth.Services
{
    public class EndpointService : CrudService<Endpoint, EndpointDto, EndpointDetailDto>, IEndpointService
    {
        private readonly IResourceService _resourceService;
        private readonly IActionService _actionService;
        public EndpointService(IRepositoryManager repositoryManager, IMapper mapper, IResourceService resourceService, IActionService actionService): base(repositoryManager, repositoryManager.Endpoint, mapper)
        {
            _resourceService = resourceService;
            _actionService = actionService;
        }

        public override async Task<EndpointDetailDto> CreateAsync(ICrudRequestDto dto)
        {
            var endpointDto = (EndpointRequestDto)dto;
            if (EndpointExists(endpointDto, out _)) throw new RequestException("Endpoint with same signature already exists!");

            return await base.CreateAsync(dto);
        }

        public async Task<SyncEndpointsResultDto> SyncEndpoints(List<EndpointRequestDto> endpoints)
        {
            var result = new SyncEndpointsResultDto();

            var resourcesUsed = new HashSet<string>(StringComparer.Ordinal);
            var actionsUsed = new HashSet<string>(StringComparer.Ordinal);
            //var resourcesActionsUsed = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            var resourceListingEndpoints = new Dictionary<string, string>(StringComparer.Ordinal);

            var endpointsMapping = await _repositoryManager.Endpoint.GetExistingEndpointsMap();
            var resourcesMapping = await _repositoryManager.Resource.GetResourcesMap(r => r.Name);
            var actionsMapping = await _repositoryManager.Action.GetDictionaryMap(a => a.Name);

            var transaction = await _repositoryManager.BeginTransactionAsync();

            // cleanup relations between resources and actions
            foreach (var resource in resourcesMapping.Values) resource.Actions.Clear();

            try
            {
                foreach (var syncEndpointDto in endpoints)
                {
                    var isEndpointUpdated = false;
                    if (syncEndpointDto.IsListingEndpoint)
                    {
                        if (resourceListingEndpoints.ContainsKey(syncEndpointDto.ResourceName)) throw new Exception($"Resource {syncEndpointDto.ResourceName} has more listing endpoints defined!");
                        resourceListingEndpoints.Add(syncEndpointDto.ResourceName, syncEndpointDto.RouteTemplate);
                    }

                    // check if to update resource
                    if (!resourcesMapping.TryGetValue(syncEndpointDto.ResourceName, out var endpointResource))
                    {
                        var dto = new ResourceRequestDto { Name = syncEndpointDto.ResourceName, DisplayName = DisplayNameHelper.GetDisplayName(syncEndpointDto.ResourceName), ListingEndpoint = syncEndpointDto.IsListingEndpoint ? syncEndpointDto.RouteTemplate : null };
                        var resourceDto = await _resourceService.CreateAsync(dto);
                        result.Resources.Added.Add(resourceDto);

                        endpointResource = await _repositoryManager.Resource.GetByKeyAsync(resourceDto.Uuid);
                        resourcesMapping.Add(syncEndpointDto.ResourceName, endpointResource);
                    }
                    resourcesUsed.Add(syncEndpointDto.ResourceName);

                    // check if to update action
                    Models.Entities.Action? endpointAction = null;
                    if (syncEndpointDto.ActionName != "ANY")
                    {
                        if (!actionsMapping.TryGetValue(syncEndpointDto.ActionName, out endpointAction))
                        {
                            var dto = new ActionRequestDto { Name = syncEndpointDto.ActionName, DisplayName = DisplayNameHelper.GetDisplayName(syncEndpointDto.ActionName) };
                            var actionDto = await _actionService.CreateAsync(dto);
                            result.Actions.Added.Add(actionDto);

                            endpointAction = await _repositoryManager.Action.GetByKeyAsync(actionDto.Uuid);
                            actionsMapping.Add(syncEndpointDto.ActionName, endpointAction);
                        }
                        actionsUsed.Add(syncEndpointDto.ActionName);
                        endpointResource.Actions.Add(endpointAction);
                    }

                    var endpointMapKey = $"{syncEndpointDto.Method} {syncEndpointDto.RouteTemplate}";
                    if (endpointsMapping.TryGetValue(endpointMapKey, out var endpoint))
                    {
                        if (!endpoint.Resource.Name.Equals(syncEndpointDto.ResourceName))
                        {
                            isEndpointUpdated = true;
                            endpoint.Resource = endpointResource;
                        }

                        if ((endpoint.Action != null && !endpoint.Action.Name.Equals(syncEndpointDto.ActionName)) || (endpoint.Action == null && endpointAction != null) || (endpoint.Action != null && endpointAction == null))
                        {
                            isEndpointUpdated = true;
                            endpoint.Action = endpointAction;
                        }

                        if (isEndpointUpdated) result.Endpoints.Updated.Add(_mapper.Map<EndpointDto>(endpoint));

                        endpointsMapping.Remove(endpointMapKey);
                    }
                    else
                    {
                        var entity = _mapper.Map<Endpoint>(syncEndpointDto);
                        entity.Resource = endpointResource;
                        entity.Action = endpointAction;

                        _repository.Create(entity);
                        result.Endpoints.Added.Add(_mapper.Map<EndpointDto>(entity));
                    }
                }

                // delete unused endpoints
                foreach (var item in endpointsMapping)
                {
                    result.Endpoints.Removed.Add(_mapper.Map<EndpointDto>(item.Value));
                    _repository.Delete(item.Value);
                }

                // update resources listing endpoints and check unused resources
                foreach (var item in resourcesMapping)
                {
                    if (item.Value.ListingEndpoint == null && resourceListingEndpoints.TryGetValue(item.Value.Name, out var listingEndpoint))
                    {
                        item.Value.ListingEndpoint = listingEndpoint;
                    }

                    if (!resourcesUsed.Contains(item.Value.Name)) result.Resources.Unused.Add(_mapper.Map<ResourceDto>(item.Value));
                }

                // TODO: what to do with permissions that are linked to removed resource-action pair

                // check unused actions
                foreach (var actionName in actionsUsed) actionsMapping.Remove(actionName);
                foreach (var item in actionsMapping) result.Actions.Unused.Add(_mapper.Map<ActionDto>(item.Value));

                await _repositoryManager.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return result;
        }

        private bool EndpointExists(EndpointRequestDto endpoint, out Endpoint? entity)
        {
            entity = _repository.FindByCondition(e => e.Method.Equals(endpoint.Method) && e.RouteTemplate.Equals(endpoint.RouteTemplate)).FirstOrDefault();

            return entity != null;
        }
    }
}
