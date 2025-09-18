using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sora.Data;
using Sora.Utils;

namespace Sora.Services;

public class AuthService(ApplicationDbContext db)
{
    public async Task<long> LogIn(string username, string password)
    {
        var result = await db.Users
            .Where(u => u.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return -1;
        }

        var passwordVerification = GlobalServices.PasswordHasher
            .VerifyHashedPassword(null, result.PasswordHash, password);

        if (passwordVerification != PasswordVerificationResult.Success)
        {
            return -1;
        }

        return result.Id;
    }
}