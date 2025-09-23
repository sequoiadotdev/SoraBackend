using Microsoft.EntityFrameworkCore;
using Sora.Data;
using Sora.DTOs;
using Sora.Models;
using Sora.Utils;

namespace Sora.Services;

public class CourseService(ApplicationDbContext db)
{
    
    public async Task<Course?> GetCourseById(long id) =>
        await db.Courses
            .Include(c => c.Lessons)
                .ThenInclude(l => l.Vocabulary)
            .Include(c => c.Lessons)
                .ThenInclude(l => l.Quizzes)
            .FirstOrDefaultAsync(c => c.Id == id);
    
    public async Task<List<Course>> GetCourses() =>
        await db.Courses
            .Include(c => c.Lessons)
                .ThenInclude(l => l.Vocabulary)
            .Include(c => c.Lessons)
                .ThenInclude(l => l.Quizzes)
            .ToListAsync();

    public async Task<long> AddCourse(CreateCourseRequest request)
    {
        var id = GlobalServices.IdGenerator.Next();
        await db.Courses.AddAsync(new Course
        {
            Id = id,
            Name = request.Title,
            Description = request.Description,
            Level = request.Level,
        });
        
        await db.SaveChangesAsync();

        return id;
    }
}
