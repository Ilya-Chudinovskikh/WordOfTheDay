using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WordOfTheDay.Repository.Entities
{
    public class WordContext : IdentityDbContext
    {
        public virtual DbSet<Word> Words { get; set; }
        public WordContext (DbContextOptions<WordContext> options)
            : base(options)
        {
        }
        public WordContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
