using Microsoft.EntityFrameworkCore;
using YellowNotes.Core.Models;

namespace YellowNotes.Core
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Note> Notes { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);
                entity.Property(e => e.PasswordHash).IsUnicode(false);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Notes)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_User");

                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Notes)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Category");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Categories)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_User");
            });
        }
    }
}
