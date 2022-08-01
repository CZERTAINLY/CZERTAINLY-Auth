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
        public async Task<ActionResult<PagedResponse<ResourceDto>>> GetResourcesAsync()
        {
            var result = await _resourceService.GetAsync(new QueryRequestDto());

            return Ok(result);
        }

        [HttpGet("{resourceUuid}")]
        public async Task<ActionResult<ResourceDetailDto>> GetUserAsync([FromRoute] Guid resourceUuid)
        {
            var entityKey = new EntityKey(resourceUuid);
            var result = await _resourceService.GetDetailAsync(entityKey);

            return Ok(result);
        }
    }
}
