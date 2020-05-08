using System.ComponentModel.DataAnnotations;

namespace YellowNotes.Core.Dtos
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
