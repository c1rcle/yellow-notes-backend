using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YellowNotes.Core.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public DateTime ModificationDate { get; set; }

        [Required]
        public string Variant { get; set; }

        [Required]
        public bool IsRemoved { get; set; }

        public string Content { get; set; }

        [ForeignKey("UserEmail")]
        [InverseProperty("Notes")]
        public virtual User User { get; set; }
    }
}
