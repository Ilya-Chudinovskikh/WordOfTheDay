using System;
using System.ComponentModel.DataAnnotations;

namespace WordOfTheDay.Models
{
    public class Word
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
