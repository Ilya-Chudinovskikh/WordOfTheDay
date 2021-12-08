using Microsoft.EntityFrameworkCore;

namespace WordOfTheDay.Models
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
