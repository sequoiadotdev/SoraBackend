using Microsoft.AspNetCore.Mvc;
using Sora.DTOs;
using Sora.Services;
using Sora.Utils;

namespace Sora.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class AuthController(ILogger<AuthController> logger, AuthService service) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LoginRequest request)
    {
        var userId = await service.LogIn(request.Username, request.Password);

        if (userId == -1)
        {
            return Unauthorized(new { Error = "Invalid username or password" });
        }
        
        return Ok(new { Token = GlobalServices.TokenManager.GenerateToken(userId) });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
    {
        var userId = await
            service.SignUp(request.Username, request.Email, request.Password);

        if (userId == -1)
        {
            return BadRequest(new { Error = "User with this email already exists" });
        }

        if (userId == -2)
        {
            return BadRequest(new { Error = "Invalid Username" });
        }

        if (userId == -3)
        {
            return BadRequest(new { Error = "Invalid/Insecure Password" });
        }

        if (userId == -4)
        {
            return BadRequest(new { Error = "Invalid Email Address" });
        }
        
        return Ok(new { Token = GlobalServices.TokenManager.GenerateToken(userId) });
    }
}