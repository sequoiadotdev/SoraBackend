using Microsoft.AspNetCore.Mvc;
using Sora.Services;

namespace Sora.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class UserController(ILogger<UserController> logger, UserService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        var userId = long.Parse(User.Claims.First().Value);
        var user = await service.GetUserById(userId);
        return Ok(new
        {
            user!.Id,
            user.Username
        });
    }
}