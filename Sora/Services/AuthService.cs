using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sora.Data;
using Sora.Models;
using Sora.Utils;

namespace Sora.Services;

public partial class AuthService(ApplicationDbContext db)
{
    private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$";
    private const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    
    public async Task<long> LogIn(string username, string password)
    {
        var result = await db.Users
            .Where(u => u.Username.ToLower() == username.ToLower())
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

    public async Task<long> SignUp(string username, string email, string password)
    {
        var result = await db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        if (result)
        {
            return -1; // Already exists
        }

        if (username.Length < 3 || username.Length > 16)
        {
            return -2; // Invalid Username
        }

        if (!PasswordRegex().IsMatch(password))
        {
            return -3; // Insecure password
        }

        if (!EmailRegex().IsMatch(email))
        {
            return -4; // Invalid Email
        }
        
        var hashedPassword = GlobalServices.PasswordHasher.HashPassword(null, password);
        var generatedId = GlobalServices.IdGenerator.Next();
        
        db.Users.Add(new User
        {
            Id = generatedId,
            Username = username,
            PasswordHash = hashedPassword,
            Email = email,
        });
        
        await db.SaveChangesAsync();
        
        return generatedId;
    }

    [GeneratedRegex(EmailPattern)]
    private static partial Regex EmailRegex();
    
    [GeneratedRegex(PasswordPattern)]
    private static partial Regex PasswordRegex();
}