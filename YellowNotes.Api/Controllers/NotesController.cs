using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;
using System.Threading;
using System.Collections.Generic;
using YellowNotes.Api.Extensions;
using Microsoft.AspNetCore.Http;

namespace YellowNotes.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService noteService;

        public NotesController(INoteService noteService) => this.noteService = noteService;

        [HttpGet("{noteId}")]
        [ProducesResponseType(typeof(NoteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoteDto>> GetNote(int noteId,
            CancellationToken cancellationToken = default)
        {
            var note = await noteService.GetNote(noteId, cancellationToken);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotes(
            [FromQuery] int takeCount = 20, [FromQuery] int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var userEmail = HttpContext.GetEmailFromClaims();
            var notes = await noteService.GetNotes(takeCount, skipCount, userEmail, cancellationToken);
            return Ok(notes);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NoteDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateNote([FromBody] NoteDto noteDto,
            CancellationToken cancellationToken = default)
        {
            var userEmail = HttpContext.GetEmailFromClaims();
            var note = await noteService.CreateNote(noteDto, userEmail, cancellationToken);

            if (note == null)
            {
                return UnprocessableEntity("Failed to create note");
            }

            return CreatedAtAction(nameof(GetNote), new { noteId = note.NoteId }, note);
        }

        [HttpPut("{noteId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
