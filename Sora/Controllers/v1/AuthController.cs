using Microsoft.AspNetCore.Mvc;
using Sora.DTOs;

namespace Sora.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class AuthController(ILogger<AuthController> logger) : ControllerBase
{

    [HttpPost("login")]
    public IActionResult LogIn([FromBody] LoginRequest request)
    {
        return Ok("Login");
    }

    [HttpPost]
    public IActionResult SignUp([FromBody] RegisterRequest request)
    {
        return Ok("SignUp");
    }
}