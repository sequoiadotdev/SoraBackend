using Microsoft.EntityFrameworkCore;
using Sora.Models;

namespace Sora.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<VocabularyItem> Vocabulary { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    private string DbPath { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VocabularyItem>()
            .HasOne<Lesson>()
            .WithMany(l => l.Vocabulary)
            .HasForeignKey(v => v.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Quiz>()
            .HasOne<Lesson>()
            .WithMany(l => l.Quizzes)
            .HasForeignKey(v => v.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<VocabularyItem>()
            .HasIndex(v => v.LessonId);

        modelBuilder.Entity<Quiz>()
            .HasIndex(v => v.LessonId);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

    }

    public ApplicationDbContext()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Combine(path, "Sora.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}