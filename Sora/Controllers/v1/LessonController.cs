using Microsoft.AspNetCore.Mvc;

namespace Sora.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class LessonController(ILogger<LessonController> logger) : ControllerBase
{
    private readonly ILogger<LessonController> _logger = logger;

    [HttpGet]
    public IActionResult Get([FromQuery] long id)
    {
        
        return Ok();
    }
}