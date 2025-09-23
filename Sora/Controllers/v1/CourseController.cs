using Microsoft.AspNetCore.Mvc;
using Sora.DTOs;
using Sora.Services;

namespace Sora.Controllers.v1;

[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/courses")]
public class CourseController(ILogger<CourseController> logger, CourseService courseService, LessonService lessonService) : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCourseById(long id)
    {
        var course = await courseService.GetCourseById(id);
        if (course is null)
        {
            return NotFound(new { Error = "Course not found" });
        }
        
        return Ok(course);
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        return Ok(new
        {
            Courses = await courseService.GetCourses()
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
    {
        return Ok(new
        {
            Id = await courseService.AddCourse(request)
        });
    }
    
    [HttpGet("{id:long}/lessons")]
    public async Task<IActionResult> GetLessonsFromCourse(long id)
    {
        var course = await courseService.GetCourseById(id);

        if (course is null)
        {
            return NotFound(new { Error = "Course not found" });
        }
        
        return Ok(course.Lessons);
    }
    
    [HttpPost("{id:long}/lessons")]
    public async Task<IActionResult> Create(long courseId, [FromBody] CreateLessonRequest lesson)
    {
        var id = await lessonService.AddLesson(courseId, lesson);
        return Ok(id);
    }

    [HttpGet("{id:long}/lessons/{lessonId:long}")]
    public async Task<IActionResult> GetLessonFromCourse(long id, long lessonId)
    {
        var course = await courseService.GetCourseById(id);

        if (course is null)
        {
            return NotFound(new { Error = "Course not found" });
        }
        
        var lesson = course.Lessons.FirstOrDefault(l => l.Id == lessonId);
        if (lesson is null)
        {
            return NotFound(new  { Error = "Lesson not found" });
        }
        
        return Ok(lesson);
    }
}