using System.ComponentModel.DataAnnotations;

namespace YellowNotes.Core.Dtos
{
    public class UserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{5,}$")]
        public string Password { get; set; }

        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{5,}$")]
        public string NewPassword { get; set; }
    }
}
