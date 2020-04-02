using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;
using System.Threading;
using System.Collections.Generic;
using YellowNotes.Api.Extensions;

namespace YellowNotes.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService noteService;

        public NotesController(INoteService noteService) => this.noteService = noteService;

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
                return UnprocessableEntity("Failed to create note");
            }
            return NoContent();
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNote(int noteId, [FromBody] NoteDto noteDto,
            CancellationToken cancellationToken = default)
        {
            noteDto.NoteId = noteId;
            var success = await noteService.UpdateNote(noteDto, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to update note");
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
                return NotFound();
            }
            return NoContent();
        }
    }
}
