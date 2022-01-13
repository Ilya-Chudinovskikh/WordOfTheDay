using System;
using System.Collections.Generic;

namespace WordOfTheDay.Repository.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
