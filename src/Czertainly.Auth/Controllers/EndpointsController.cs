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

        [HttpGet("{endpointUuid}")]
        public async Task<ActionResult<EndpointDetailDto>> GetEndpointAsync([FromRoute] Guid endpointUuid)
        {
            var result = await _endpointService.GetDetailAsync(endpointUuid);

            return Ok(result);
        }

        [HttpPost("sync")]
        //[ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<SyncEndpointsResultDto>> SyncEndpointsAsync([FromBody] List<EndpointRequestDto> syncEndpoints)
        {
            var result = await _endpointService.SyncEndpoints(syncEndpoints);

            return Ok(result);
        }
    }
}
