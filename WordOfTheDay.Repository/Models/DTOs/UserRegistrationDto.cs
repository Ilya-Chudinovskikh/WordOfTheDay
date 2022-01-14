using System;
using System.ComponentModel.DataAnnotations;

namespace WordOfTheDay.Repository.Models.DTOs
{
    public class UserRegistrationDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
