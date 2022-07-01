using Czertainly.Auth.Common.Filters;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Czertainly.Auth.Controllers
{
    [ApiVersion("1.0")]
    [Route("auth/endpoints")]
    [ApiController]
    public class EndpointsController : ControllerBase
    {
        private readonly IEndpointService _endpointService;

        public EndpointsController(IEndpointService endpointService)
        {
            _endpointService = endpointService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<EndpointDto>>> GetEndpointsAsync()
        {
            var result = await _endpointService.GetAsync(new QueryRequestDto());

            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<EndpointDto>> CreateEndpointAsync([FromBody] EndpointRequestDto endpointRequestDto)
        {
            var result = await _endpointService.CreateAsync(endpointRequestDto);

            return Created("auth/endpoints", result);
        }

        [HttpGet("{endpointUuid}")]
        public async Task<ActionResult<EndpointDto>> GetEndpointAsync([FromRoute] Guid endpointUuid)
        {
            var entityKey = new EntityKey { Uuid = endpointUuid };
            var result = await _endpointService.GetDetailAsync(entityKey);

            return Ok(result);
        }

        [HttpPut("{endpointUuid}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<EndpointDto>> UpdateEndpointAsync([FromRoute] Guid endpointUuid, [FromBody] EndpointRequestDto endpointRequestDto)
        {
            var entityKey = new EntityKey { Uuid = endpointUuid };
            var result = await _endpointService.UpdateAsync(entityKey, endpointRequestDto);

            return Ok(result);
        }

        [HttpDelete("{endpointUuid}")]
        public async Task<ActionResult<EndpointDto>> DeleteEndpointAsync([FromRoute] Guid endpointUuid)
        {
            var entityKey = new EntityKey { Uuid = endpointUuid };
            await _endpointService.DeleteAsync(entityKey);

            return NoContent();
        }

        [HttpPost("sync")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<EndpointDto>> SyncEndpointsAsync([FromBody] List<EndpointRequestDto> syncEndpoints)
        {
            var result = await _endpointService.SyncEndpoints(syncEndpoints);

            return Ok(result);
        }
    }
}
