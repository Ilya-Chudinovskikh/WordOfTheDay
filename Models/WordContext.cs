using System;
using Microsoft.EntityFrameworkCore;

namespace WordOfTheDay.Models
{
    public class WordContext : DbContext
    {
        public WordContext (DbContextOptions<WordContext> options)
            : base(options)
        {
            if (DateTime.Now.ToString("HH:mm") == "00:00")
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
        public DbSet<Word> Words { get; set; }
    }
}