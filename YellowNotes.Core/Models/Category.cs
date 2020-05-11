using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YellowNotes.Core.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Categories")]
        public virtual User User { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Note> Notes { get; set; }
    }
}
