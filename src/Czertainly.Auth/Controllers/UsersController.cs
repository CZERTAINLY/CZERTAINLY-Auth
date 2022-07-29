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
        public async Task<ActionResult<UserProfileDto>> GetUserProfile([FromHeader] UserProfileRequestDto dto)
        {
            var result = await _userService.GetUserProfileAsync(dto);

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
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] UserRequestDto userRequestDto)
        {
            var result = await _userService.CreateAsync(userRequestDto);

            return Created("auth/users", result);
        }

        [HttpGet("{userUuid}")]
        public async Task<ActionResult<UserDetailDto>> GetUserAsync([FromRoute] Guid userUuid)
        {
            var entityKey = new EntityKey(userUuid);
            var result = await _userService.GetDetailAsync(entityKey);

            return Ok(result);
        }

        [HttpPut("{userUuid}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<UserDto>> UpdateUserAsync([FromRoute] Guid userUuid, [FromBody] UserRequestDto userRequestDto)
        {
            var entityKey = new EntityKey(userUuid);
            var result = await _userService.UpdateAsync(entityKey, userRequestDto);

            return Ok(result);
        }

        [HttpDelete("{userUuid}")]
        public async Task<ActionResult<UserDto>> DeleteUserAsync([FromRoute] Guid userUuid)
        {
            var entityKey = new EntityKey(userUuid);
            await _userService.DeleteAsync(entityKey);

            return NoContent();
        }

        [HttpPatch("{userUuid}/roles")]
        public async Task<ActionResult<UserDetailDto>> AssignRolesAsync([FromRoute] Guid userUuid, [FromBody] IEnumerable<Guid> roleUuids)
        {
            var result = await _userService.AssignRolesAsync(new EntityKey(userUuid), roleUuids);

            return Ok(result);
        }

        [HttpPut("{userUuid}/roles/{roleUuid}")]
        public async Task<ActionResult<UserDetailDto>> AssignRoleAsync([FromRoute] Guid userUuid, [FromRoute] Guid roleUuid)
        {
            var result = await _userService.AssignRoleAsync(new EntityKey(userUuid), new EntityKey(roleUuid));

            return Ok(result);
        }
    }
}
