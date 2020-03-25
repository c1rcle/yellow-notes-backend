using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;
using YellowNotes.Core.Utility;
using System.Threading;

namespace YellowNotes.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService) => this.userService = userService;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto,
            CancellationToken cancellationToken = default)
        {
            var success = await userService.CreateUser(userDto, cancellationToken);
            if (!success)
            {
                return BadRequest("User cannot be created");
            }

            var token = userService.GenerateJWT(userDto);
            return Ok(new { token });
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto userDto,
            CancellationToken cancellationToken = default)
        {
            var success = await userService.VerifyPassword(userDto, cancellationToken);
            if (!success)
            {
                return BadRequest("Verification has failed");
            }

            var token = userService.GenerateJWT(userDto);
            return Ok(new { token });
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] UserDto userDto,
            CancellationToken cancellationToken = default)
        {
            var success = await userService.ChangePassword(userDto, cancellationToken);
            if (!success)
            {
                return BadRequest("Failed to change password");
            }

            return Ok();
        }
    }
}
