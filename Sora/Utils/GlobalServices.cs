using Microsoft.AspNetCore.Identity;

namespace Sora.Utils;

public static class GlobalServices
{
    public static readonly IdGenerator IdGenerator = new(1, 1);
    public static readonly TokenManager TokenManager = new(
        Convert.FromBase64String(Environment.GetEnvironmentVariable("TokenSigningKey")!)
    );
    public static PasswordHasher<object> PasswordHasher = new();
}