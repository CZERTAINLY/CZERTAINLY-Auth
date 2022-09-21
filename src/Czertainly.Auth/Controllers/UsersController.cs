using Czertainly.Auth.Common.Filters;
using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Czertainly.Auth.Controllers
{
    [ApiVersion("1.0")]
    [Route("auth/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<AuthenticationResponseDto>> GetUserProfile([FromHeader(Name = "x-app-certificate")] string certificate)
        {
            var result = await _userService.AuthenticateUserAsync(certificate);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<UserDto>>> GetUsersAsync()
        {
            var result = await _userService.GetAsync(new QueryRequestDto());

            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<UserDetailDto>> CreateUserAsync([FromBody] UserRequestDto userRequestDto)
        {
            var result = await _userService.CreateAsync(userRequestDto);

            return Created("auth/users", result);
        }

        [HttpGet("{userUuid}")]
        public async Task<ActionResult<UserDetailDto>> GetUserAsync([FromRoute] Guid userUuid)
        {
            var result = await _userService.GetDetailAsync(userUuid);

            return Ok(result);
        }

        [HttpPut("{userUuid}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<UserDetailDto>> UpdateUserAsync([FromRoute] Guid userUuid, [FromBody] UserUpdateRequestDto userRequestDto)
        {
            var result = await _userService.UpdateAsync(userUuid, userRequestDto);

            return Ok(result);
        }

        [HttpDelete("{userUuid}")]
        public async Task<ActionResult> DeleteUserAsync([FromRoute] Guid userUuid)
        {
            await _userService.DeleteAsync(userUuid);

            return NoContent();
        }

        [HttpPatch("{userUuid}/enable")]
        public async Task<ActionResult<UserDetailDto>> EnableUserAsync([FromRoute] Guid userUuid)
        {
            var result = await _userService.EnableUserAsync(userUuid, true);

            return Ok(result);
        }

        [HttpPatch("{userUuid}/disable")]
        public async Task<ActionResult<UserDetailDto>> DisableUserAsync([FromRoute] Guid userUuid)
        {
            var result = await _userService.EnableUserAsync(userUuid, false);

            return Ok(result);
        }

        [HttpGet("{userUuid}/roles")]
        public async Task<ActionResult<List<RoleDto>>> GetUserRolesAsync([FromServices] IRoleService roleService, [FromRoute] Guid userUuid)
        {
            var result = await roleService.GetUserRolesAsync(userUuid);

            return Ok(result);
        }

        [HttpPatch("{userUuid}/roles")]
        public async Task<ActionResult<UserDetailDto>> AssignRolesAsync([FromRoute] Guid userUuid, [FromBody] IEnumerable<Guid> roleUuids)
        {
            var result = await _userService.AssignRolesAsync(userUuid, roleUuids);

            return Ok(result);
        }

        [HttpPut("{userUuid}/roles/{roleUuid}")]
        public async Task<ActionResult<UserDetailDto>> AssignRoleAsync([FromRoute] Guid userUuid, [FromRoute] Guid roleUuid)
        {
            var result = await _userService.AssignRoleAsync(userUuid, roleUuid);

            return Ok(result);
        }

        [HttpDelete("{userUuid}/roles/{roleUuid}")]
        public async Task<ActionResult<UserDetailDto>> RemoveRoleAsync([FromRoute] Guid userUuid, [FromRoute] Guid roleUuid)
        {
            var result = await _userService.RemoveRoleAsync(userUuid, roleUuid);

            return Ok(result);
        }
    }
}
