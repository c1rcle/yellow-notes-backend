using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;
namespace YellowNotes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService) => this.userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            bool success = await this.userService.CreateUser(userDto);
            if (!success)
                return BadRequest();

            return Ok();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> VerifyPassword([FromBody] UserDto userDto)
        {
            bool success = await this.userService.VerifyPassword(userDto);
            if (!success)
                return BadRequest();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] UserDto userDto)
        {
            bool success = await this.userService.VerifyPassword(userDto);
            if (!success)
                return BadRequest();

            return Ok();
        }
    }
}
