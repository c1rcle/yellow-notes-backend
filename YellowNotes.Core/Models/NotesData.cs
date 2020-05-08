using System.Collections.Generic;

namespace YellowNotes.Core.Models
{
    public class NotesData
    {
        public int Count { get; set; }

        public IEnumerable<Note> Notes { get; set; }
    }
}
