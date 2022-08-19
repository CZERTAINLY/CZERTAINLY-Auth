using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Czertainly.Auth.Controllers
{
    [ApiVersion("1.0")]
    [Route("auth/resources")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourcesController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<ResourceDto>>> GetResourcesAsync([FromQuery] QueryRequestDto query)
        {
            var result = await _resourceService.GetAsync(query);

            return Ok(result);
        }

        [HttpGet("{resourceUuid}")]
        public async Task<ActionResult<ResourceDetailDto>> GetResourceAsync([FromRoute] Guid resourceUuid)
        {
            var result = await _resourceService.GetDetailAsync(resourceUuid);

            return Ok(result);
        }
    }
}
