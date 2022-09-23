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
        public async Task<ActionResult<List<ResourceDetailDto>>> GetResourcesAsync()
        {
            var result = await _resourceService.GetAllResourcesAsync();

            return Ok(result);
        }

        [HttpPost("sync")]
        public async Task<ActionResult<SyncResourcesResponseDto>> SyncResourcesAsync([FromBody] List<ResourceSyncRequestDto> resources)
        {
            var result = await _resourceService.SyncResourcesAsync(resources);

            return Ok(result);
        }
    }
}
