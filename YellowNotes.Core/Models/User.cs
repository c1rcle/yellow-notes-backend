using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YellowNotes.Core.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Note> Notes { get; set; }
    }
}
