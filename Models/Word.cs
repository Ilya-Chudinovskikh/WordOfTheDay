using System;
using System.ComponentModel.DataAnnotations;

namespace WordOfTheDay.Models
{
    public class Word
    {
        public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Text { get; set; }
        [Required]
        public DateTime AddTime { get; set; }
    }
}
