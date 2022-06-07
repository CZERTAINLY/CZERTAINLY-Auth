using Czertainly.Auth.Common.Models.Dto;
using Czertainly.Auth.Models.Dto;
using Czertainly.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Czertainly.Auth.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/auth/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PagedResponse<UserDto>>> Get()
        {
            var result = await _userService.GetUsersAsync();

            return Ok(result);
        }
    }
}
