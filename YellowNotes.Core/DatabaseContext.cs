using Microsoft.EntityFrameworkCore;
using YellowNotes.Core.Models;

namespace YellowNotes.Core
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            :base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);
                entity.Property(e => e.PasswordHash).IsUnicode(false); 
            });

            modelBuilder.Entity<Note>(entity => 
            {
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Notes)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_User");
            });
        }
    }
}
