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
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
        public async Task<ActionResult<UserDto>> GetUserAsync([FromRoute] Guid userUuid)
        {
            var entityKey = new EntityKey { Uuid = userUuid };
            var result = await _userService.GetDetailAsync(entityKey);

            return Ok(result);
        }

        [HttpPut("{userUuid}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<UserDto>> UpdateUserAsync([FromRoute] Guid userUuid, [FromBody] UserRequestDto userRequestDto)
        {
            var entityKey = new EntityKey { Uuid = userUuid };
            var result = await _userService.UpdateAsync(entityKey, userRequestDto);

            return Ok(result);
        }

        [HttpDelete("{userUuid}")]
        public async Task<ActionResult<UserDto>> DeleteUserAsync([FromRoute] Guid userUuid)
        {
            var entityKey = new EntityKey { Uuid = userUuid };
            await _userService.DeleteAsync(entityKey);

            return NoContent();
        }

        [HttpGet("{userUuid}/roles")]
        public async Task<ActionResult<UserDto>> GetRoles([FromRoute] Guid userUuid)
        {
            var entityKey = new EntityKey { Uuid = userUuid };
            var result = await _userService.UpdateAsync(entityKey, userRequestDto);

            return Ok(result);
        }

        [HttpPatch("{userUuid}/roles")]
        public async Task<ActionResult<UserDto>> AssignRoles([FromRoute] Guid userUuid, [FromBody] List<Guid> roleUuids)
        {
            var entityKey = new EntityKey { Uuid = userUuid };
            var result = await _userService.UpdateAsync(entityKey, userRequestDto);

            return Ok(result);
        }

        [HttpPut("{userUuid}/roles/{roleUuid}")]
        public async Task<ActionResult<UserDto>> AssignRole([FromRoute] Guid userUuid, [FromBody] UserRequestDto userRequestDto)
        {
            var entityKey = new EntityKey { Uuid = userUuid };
            var result = await _userService.UpdateAsync(entityKey, userRequestDto);

            return Ok(result);
        }
    }
}
