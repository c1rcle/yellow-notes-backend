using Microsoft.EntityFrameworkCore;

namespace YellowNotes.Core
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            :base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);
                entity.Property(e => e.PasswordHash).IsUnicode(false); 
            });
        }
    }
}
