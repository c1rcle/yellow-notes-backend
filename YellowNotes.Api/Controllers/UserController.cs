using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;
using System.Threading;
using YellowNotes.Core.Utility;

namespace YellowNotes.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        private readonly IEmailService emailService;

        public UserController(IUserService userService, IEmailService emailService) 
        {
            this.userService = userService;
            this.emailService = emailService;
        }

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

            try
            {
                await emailService.SendEmail(EmailGenerator
                    .RegistrationMessage(userDto.Email), cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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

            await emailService.SendEmail(EmailGenerator
                .PasswordChangeMessage(userDto.Email), cancellationToken);

            return NoContent();
        }
    }
}
