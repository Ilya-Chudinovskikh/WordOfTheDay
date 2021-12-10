using System;
using System.ComponentModel.DataAnnotations;

namespace WordOfTheDay.Models
{
    public class Word
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Enter your email!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter your word!")]
        [StringLength(50, ErrorMessage = "Your word must be no more than 50 characters long!")]
        public string Text { get; set; }
        [Required]
        public DateTime AddTime { get; set; }
    }
}
