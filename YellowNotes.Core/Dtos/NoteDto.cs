using System;
using System.ComponentModel.DataAnnotations;

namespace YellowNotes.Core.Dtos
{
    public class NoteDto
    {
        public int NoteId { get; set; }

        public DateTime ModificationDate { get; set; }

        [Required]
        [StringLength(4)]
        public string Variant { get; set; }

        public string Content { get; set; }
    }
}
