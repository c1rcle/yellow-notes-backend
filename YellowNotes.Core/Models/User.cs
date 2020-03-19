using System;
using System.ComponentModel.DataAnnotations;


namespace YellowNotes.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime AccountCreationDate { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
