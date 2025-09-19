using Sora.Data;
using Sora.Models;

namespace Sora.Services;

public class UserService(ApplicationDbContext db)
{
    public async Task<User?> GetUserById(long id)
    {
        return await db.Users.FindAsync(id);
    }
}