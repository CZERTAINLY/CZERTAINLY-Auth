using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Czertainly.Auth.Controllers
{
    [ApiVersion("1.0")]
    [Route("auth/permissions")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("/users/{userUuid}/permissions")]
        public async Task<ActionResult<MergedPermissionsDto>> GetUserPermissionsAsync([FromRoute] Guid userUuid)
        {
            var result = await _permissionService.GetUserPermissionsAsync(userUuid);

            return Ok(result);
        }
    }
}
