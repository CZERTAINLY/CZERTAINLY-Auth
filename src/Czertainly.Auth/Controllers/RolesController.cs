using Czertainly.Auth.Common.Filters;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Czertainly.Auth.Controllers
{
    [ApiVersion("1.0")]
    [Route("auth/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<RoleDto>>> GetRolesAsync()
        {
            var result = await _roleService.GetAsync(new QueryRequestDto());

            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<RoleDto>> CreateRoleAsync([FromBody] RoleRequestDto roleRequestDto)
        {
            var result = await _roleService.CreateAsync(roleRequestDto);

            return Created("auth/roles", result);
        }

        [HttpGet("{roleUuid}")]
        public async Task<ActionResult<RoleDetailDto>> GetRoleAsync([FromRoute] Guid roleUuid)
        {
            var entityKey = new EntityKey(roleUuid);
            var result = await _roleService.GetDetailAsync(entityKey);

            return Ok(result);
        }

        [HttpPut("{roleUuid}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<RoleDto>> UpdateRoleAsync([FromRoute] Guid roleUuid, [FromBody] RoleRequestDto roleRequestDto)
        {
            var entityKey = new EntityKey(roleUuid);
            var result = await _roleService.UpdateAsync(entityKey, roleRequestDto);

            return Ok(result);
        }

        [HttpDelete("{roleUuid}")]
        public async Task<ActionResult<RoleDto>> DeleteRoleAsync([FromRoute] Guid roleUuid)
        {
            var entityKey = new EntityKey(roleUuid);
            await _roleService.DeleteAsync(entityKey);

            return NoContent();
        }

    }
}
