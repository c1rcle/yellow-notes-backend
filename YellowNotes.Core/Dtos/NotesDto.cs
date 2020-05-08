using System.Collections.Generic;

namespace YellowNotes.Core.Dtos
{
    public class NotesDto
    {
        public int Count { get; set; }

        public IEnumerable<NoteDto> Notes { get; set; }
    }
}
