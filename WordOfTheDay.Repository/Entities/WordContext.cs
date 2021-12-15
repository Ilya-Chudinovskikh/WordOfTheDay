using System;
using Microsoft.EntityFrameworkCore;

namespace WordOfTheDay.Entities
{
    public class WordContext : DbContext
    {
        public WordContext (DbContextOptions<WordContext> options)
            : base(options)
        {
        }
        public DbSet<Word> Words { get; set; }
    }
}
