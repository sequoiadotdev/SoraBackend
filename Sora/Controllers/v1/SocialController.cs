using Microsoft.AspNetCore.Mvc;

namespace Sora.Controllers.v1;
[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class SocialController : ControllerBase
{
    
}