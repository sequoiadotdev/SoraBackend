using System.Security.Claims;

namespace Sora.Utils;

public class SoraAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token))
        {
            var result = TokenManager.VerifyToken(token);

            if (result.Valid)
            {
                context.Items["UserId"] = result.UserId;
                var claims = new List<Claim> { new("UserId", result.UserId.ToString()) };
                context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));
            }
        }

        await next(context);
    }
}