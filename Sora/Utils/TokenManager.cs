using System.Security.Cryptography;
using System.Text;

namespace Sora.Utils;

public class TokenManager
{
    private static HMACSHA256? _hmacsha256;
    private const long Epoch = 1756684800;
    public TokenManager(byte[] key)
    {
        _hmacsha256 = new HMACSHA256(key);
    }
    
    public string GenerateToken(long userId)
    {
        var userIdEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString()))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var expiresAt = now + (14 * 24 * 60 * 60);
        var timeEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(expiresAt.ToString()))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        
        var signData = userIdEncoded + "." + timeEncoded;
        var signature = _hmacsha256!.ComputeHash(Encoding.UTF8.GetBytes(signData));
        var signatureEncoded = Convert.ToBase64String(signature)
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        
        return "Sora." + signData + "." + signatureEncoded;
    }
    
    public static TokenVerificationResult VerifyToken(string token)
    {
        if (!token.StartsWith("Sora."))
        {
            return new TokenVerificationResult(token, false, 0, 0);
        }
        
        var parts = token[5..].Split('.');
        if (_hmacsha256 == null || parts.Length != 3)
        {
            return new TokenVerificationResult(token, false, 0, 0);
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        try
        {
            var userIdEncoded = parts[0].Replace('-', '+').Replace('_', '/');
            switch (userIdEncoded.Length % 4)
            {
                case 2: userIdEncoded += "=="; break;
                case 3: userIdEncoded += "="; break;
            }
            
            var timeEncoded = parts[1].Replace('-', '+').Replace('_', '/');
            switch (timeEncoded.Length % 4)
            {
                case 2: timeEncoded += "=="; break;
                case 3: timeEncoded += "="; break;
            }
            
            var signatureEncoded = parts[2].Replace('-', '+').Replace('_', '/');
            switch (signatureEncoded.Length % 4)
            {
                case 2: signatureEncoded += "=="; break;
                case 3: signatureEncoded += "="; break;
            }
            var signature = Convert.FromBase64String(signatureEncoded);
            
            var userId = Convert.ToInt64(Encoding.UTF8.GetString(Convert.FromBase64String(userIdEncoded)));
            var time = Convert.ToInt64(Encoding.UTF8.GetString(Convert.FromBase64String(timeEncoded)));
            
            var signData = parts[0] + "." + parts[1];
            var computed = _hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(signData));

            return CryptographicOperations.FixedTimeEquals(signature, computed) 
                ? new TokenVerificationResult(token, now <= time, userId, time) 
                : new TokenVerificationResult(token, false, userId, time);
        }
        catch
        {
            return new TokenVerificationResult(token, false, 0, 0);
        }
    }
    
    public record TokenVerificationResult(string Token, bool Valid, long UserId, long Expiration);
}