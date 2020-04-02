using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;
using System.Threading;
using YellowNotes.Core.Utility;
using Microsoft.AspNetCore.Http;

namespace YellowNotes.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        private readonly IEmailService emailService;

        public UsersController(IUserService userService, IEmailService emailService)
        {
            this.userService = userService;
            this.emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register([FromBody] UserDto userDto,
            CancellationToken cancellationToken = default)
        {
            var success = await userService.CreateUser(userDto, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("User cannot be created");
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

            var token = userService.GenerateJwt(userDto);
            return Ok(new { token });
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authenticate([FromBody] UserDto userDto,
            CancellationToken cancellationToken = default)
        {
            var success = await userService.VerifyPassword(userDto, cancellationToken);
            if (!success)
            {
                return Unauthorized("Verification has failed");
            }

            var token = userService.GenerateJwt(userDto);
            return Ok(new { token });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangePassword([FromBody] UserDto userDto,
            CancellationToken cancellationToken = default)
        {
            var success = await userService.ChangePassword(userDto, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to change password");
            }

            await emailService.SendEmail(EmailGenerator
                .PasswordChangeMessage(userDto.Email), cancellationToken);

            return NoContent();
        }
    }
}
