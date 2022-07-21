using AutoMapper;
using Czertainly.Auth.Common.Exceptions;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Common.Services;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Dto;
using Endpoint = Czertainly.Auth.Models.Entities.Endpoint;

namespace Czertainly.Auth.Services
{
    public class EndpointService : BaseResourceService<Endpoint, EndpointDto>, IEndpointService
    {

        public EndpointService(IRepositoryManager repositoryManager, IMapper mapper): base(repositoryManager, repositoryManager.Endpoint, mapper)
        {
        }

        public override async Task<EndpointDto> CreateAsync(IRequestDto dto)
        {
            var endpointDto = (EndpointRequestDto)dto;
            if (EndpointExists(endpointDto, out var storedEntity)) throw new RequestException("Endpoint with same signature already exists!");

            return await base.CreateAsync(dto);
        }

        public async Task<EndpointsSyncResultDto> SyncEndpoints(List<EndpointRequestDto> endpoints)
        {
            EndpointDto endpointDto;
            var result = new EndpointsSyncResultDto();

            var resourcesMapping = await _repositoryManager.Resource.GetDictionaryMap(r => r.Name);
            var actionsMapping = await _repositoryManager.Action.GetDictionaryMap(a => $"{a.ResourceId}.{a.Name}");
            var endpointsMapping = await _repositoryManager.Endpoint.GetDictionaryMap(e => $"{e.Method} {e.RouteTemplate}");
            foreach (var endpointRequestDto in endpoints)
            {
                var endpointMapKey = $"{endpointRequestDto.Method} {endpointRequestDto.RouteTemplate}";
                if (endpointsMapping.TryGetValue(endpointMapKey, out var storedEntity))
                {
                    // check if to update resource and action
                    endpointDto = await UpdateAsync(new EntityKey { Id = storedEntity.Id, Uuid = storedEntity.Uuid }, endpointRequestDto);
                    result.UpdatedEndpoints.Add(endpointDto);
                }
                else
                {
                    endpointDto = await base.CreateAsync(endpointRequestDto);
                    result.AddedEndpoints.Add(endpointDto);
                }
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
