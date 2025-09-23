using Microsoft.AspNetCore.Mvc;
using Sora.DTOs;
using Sora.Models;
using Sora.Services;

namespace Sora.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/lessons")]
public class LessonController(ILogger<LessonController> logger, LessonService service) : ControllerBase
{

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var lesson = await service.GetLessonById(id);
        if (lesson is null)
        {
            return NotFound();
        }
        
        return Ok(lesson);
    }

    [HttpGet("{id:long}/vocabulary")]
    public async Task<IActionResult> GetVocabulary(long id)
    {
        var lesson = await service.GetLessonById(id);

        if (lesson is null)
        {
            return NotFound(new { Error = "Lesson not found" });
        }

        return Ok(lesson.Vocabulary);
    }

    [HttpGet("{id:long}/quizzes")]
    public async Task<IActionResult> GetQuizzes(long id)
    {
        var lesson = await service.GetLessonById(id);

        if (lesson is null)
        {
            return NotFound(new { Error = "Lesson not found" });
        }
        
        return Ok(lesson.Quizzes);
    }
}