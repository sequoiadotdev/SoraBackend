using Microsoft.EntityFrameworkCore;
using Sora.Data;
using Sora.DTOs;
using Sora.Models;
using Sora.Utils;

namespace Sora.Services;

public class LessonService(ApplicationDbContext db)
{
    
    public async Task<Lesson?> GetLessonById(long id) =>
        await db.Lessons
            .Include(l => l.Quizzes)
            .Include(l => l.Vocabulary)
            .FirstOrDefaultAsync(l => l.Id == id);
    
    public async Task<List<Lesson>> GetAllLessons() =>
        await db.Lessons
            .Include(l => l.Quizzes)
            .Include(l => l.Vocabulary)
            .ToListAsync();

    public async Task<long> AddLesson(long courseId, CreateLessonRequest request)
    {
        var id = GlobalServices.IdGenerator.Next();
        await db.AddAsync(new Lesson
        {
            Id = id,
            CourseId = courseId,
            Title = request.Title,
            Description = request.Description,
        });
        
        await db.SaveChangesAsync();
        
        return id;
    }
}