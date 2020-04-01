using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;
using System.Threading;
using YellowNotes.Core.Utility;
using System.Collections.Generic;

namespace YellowNotes.Api.Controllers
{
    [ApiController]
    [Route("notes")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService noteService;


        public NoteController(INoteService noteService)
        {
            this.noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotes([FromBody] UserDto userDto, [FromQuery] int takeCount, [FromQuery] int skipCount,
            CancellationToken cancellationToken = default)
        {
            var httpHeaders = Request.Headers;
            var errorMessage = TokenUtility.Authorize(userDto, httpHeaders);
            if (errorMessage != null)
            {
                return Unauthorized(errorMessage);
            }

            var userEmail = TokenUtility.DecodeEmail(userDto, httpHeaders);
            var notes = await noteService.GetNotes(takeCount, skipCount, userEmail, cancellationToken);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] UserDto userDto, [FromBody] NoteDto noteDto,
            CancellationToken cancellationToken = default)
        {
            var httpHeaders = Request.Headers;
            var errorMessage = TokenUtility.Authorize(userDto, httpHeaders);
            if (errorMessage != null)
            {
                return Unauthorized(errorMessage);
            }

            var userEmail = TokenUtility.DecodeEmail(userDto, httpHeaders);
            var success = await noteService.CreateNote(noteDto, userEmail, cancellationToken);
            if (!success)
            {
                return BadRequest("Failed to create note");
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote([FromBody] UserDto userDto, [FromBody] NoteDto noteDto,
            CancellationToken cancellationToken = default)
        {
            var httpHeaders = Request.Headers;
            var errorMessage = TokenUtility.Authorize(userDto, httpHeaders);
            if (errorMessage != null)
            {
                return Unauthorized(errorMessage);
            }

            var success = await noteService.UpdateNote(noteDto, cancellationToken);
            if (!success)
            {
                return BadRequest("Failed to update note");
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNote([FromBody] UserDto userDto, [FromQuery] int noteId,
            CancellationToken cancellationToken = default)
        {
            var httpHeaders = Request.Headers;
            var errorMessage = TokenUtility.Authorize(userDto, httpHeaders);
            if (errorMessage != null)
            {
                return Unauthorized(errorMessage);
            }

            var success = await noteService.DeleteNote(noteId, cancellationToken);
            if (!success)
            {
                return BadRequest("Failed to delete note");
            }
            return Ok();
        }
    }
}
