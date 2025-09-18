using Microsoft.AspNetCore.Mvc;
using Sora.Models;
using Sora.Services;

namespace Sora.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class LessonController(ILogger<LessonController> logger, LessonService service) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var lesson = await service.GetLessonById(id);
        if (lesson == null)
        {
            return NotFound();
        }
        
        return Ok(lesson);
    }
}