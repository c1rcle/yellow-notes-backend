using System;
using System.ComponentModel.DataAnnotations;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Dtos
{
    public class NoteDto
    {
        public int NoteId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime ModificationDate { get; set; }

        [Required]
        public NoteVariant Variant { get; set; } = NoteVariant.Text;

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string Color { get; set; }

        public bool IsBlocked { get; set; }
    }
}
