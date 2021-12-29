using Microsoft.EntityFrameworkCore;

namespace WordOfTheDay.Repository.Entities
{
    public class WordContext : DbContext
    {
        public WordContext (DbContextOptions<WordContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Word> Words { get; set; }
        public WordContext()
        {
        }
    }
}
