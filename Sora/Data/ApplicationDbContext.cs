using Microsoft.EntityFrameworkCore;
using Sora.Models;

namespace Sora.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<VocabularyItem> Vocabulary { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupMessage> GroupMessages { get; set; }
    private string DbPath { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lesson>()
            .HasMany<Course>()
            .WithMany(c => c.Lessons);
        
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

        modelBuilder.Entity<Lesson>()
            .HasIndex(l => l.Id);
        
        modelBuilder.Entity<Quiz>()
            .HasIndex(v => v.LessonId);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<Friend>()
            .HasIndex(f => new { f.UserId, f.FriendUserId })
            .IsUnique();

        modelBuilder.Entity<GroupMember>()
            .HasIndex(gm => new { gm.GroupId, gm.UserId })
            .IsUnique();
    }

    public ApplicationDbContext()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Combine(path, "Sora.db");
        Console.WriteLine($"{DbPath}");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}