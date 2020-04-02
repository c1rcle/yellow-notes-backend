using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;
using System.Threading;
using YellowNotes.Core.Utility;
using System.Collections.Generic;
using YellowNotes.Api.Extensions;

namespace YellowNotes.Api.Controllers
{
    [ApiController]
    [Route("notes")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService noteService;

        public NoteController(INoteService noteService) => this.noteService = noteService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotes(
            [FromQuery] int takeCount = 20, [FromQuery] int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var userEmail = HttpContext.GetEmailFromClaims();
            var notes = await noteService.GetNotes(takeCount, skipCount, userEmail, cancellationToken);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteDto noteDto,
            CancellationToken cancellationToken = default)
        {
            var userEmail = HttpContext.GetEmailFromClaims();
            
            var success = await noteService.CreateNote(noteDto, userEmail, cancellationToken);
            if (!success)
            {
                return BadRequest("Failed to create note");
            }
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote([FromBody] NoteDto noteDto,
            CancellationToken cancellationToken = default)
        {
            var success = await noteService.UpdateNote(noteDto, cancellationToken);
            if (!success)
            {
                return BadRequest("Failed to update note");
            }
            return NoContent();
        }

        [HttpDelete("{noteId}")]
        public async Task<IActionResult> DeleteNote(int noteId,
            CancellationToken cancellationToken = default)
        {
            var success = await noteService.DeleteNote(noteId, cancellationToken);
            if (!success)
            {
                return BadRequest("Failed to delete note");
            }
            return NoContent();
        }
    }
}
