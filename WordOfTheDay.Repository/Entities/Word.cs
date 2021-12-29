using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace WordOfTheDay.Repository.Entities
{
    [Index("Text", IsUnique = true, Name = "Text_Index")]
    public class Word
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Enter your email!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter your word!")]
        [StringLength(50, ErrorMessage = "Your word must be no more than 50 characters long!")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Your word must be in English, without using spaces and special characters!")]
        public string Text { get; set; }
        [Required]
        public DateTime AddTime { get; set; }
    }
}
