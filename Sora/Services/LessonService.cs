using Microsoft.EntityFrameworkCore;
using Sora.Data;
using Sora.Models;

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
}