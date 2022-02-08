using Microsoft.EntityFrameworkCore;

namespace WordOfTheDay.Repository.Entities
{
    public class WordContext : DbContext
    {
        public WordContext (DbContextOptions<WordContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public virtual DbSet<Word> Words { get; set; }
        public WordContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>()
                .HasIndex(w => new { w.AddTime, w.Email })
                .HasDatabaseName("DateEmail_Index")
                .IsUnique();

            modelBuilder.Entity<Word>()
                .HasIndex(w => new { w.AddTime, w.Text })
                .HasDatabaseName("DateText_Index");
        }
    }
}
