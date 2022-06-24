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

            return Ok(result);
        }
    }
}
