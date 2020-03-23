using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;
using YellowNotes.Core.Utility;
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
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("User data is not valid");

            bool success = await userService.CreateUser(userDto);
            if (!success)
                return BadRequest("User cannot be created");

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("User data is not valid");

            bool success = await userService.VerifyPassword(userDto);
            if (!success)
                return BadRequest("Verification has failed");

            string token = userService.GenerateJWT(userDto);

            return Ok(new { token });
        }

        [HttpGet]
        public IActionResult GetSomeTestContent([FromBody] UserDto userDto)
        {
            var httpHeaders = Request.Headers;

            string token = TokenParser.ParseFromHeader(httpHeaders);
            if(token == null)
                return BadRequest("No token");

            bool valid = userService.ValidateToken(token, userDto);
            if (!valid)
                return Unauthorized("Bad token");

            return Ok("Authorized access to test function");
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("User data is not valid");

            bool success = await userService.ChangePassword(userDto);
            if (!success)
                return BadRequest("Failed to change password");

            return Ok();
        }
    }
}
