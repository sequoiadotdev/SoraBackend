namespace Sora.Models;

public class Friend
{
    public long Id { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public long FriendUserId { get; set; }
    public User FriendUser { get; set; }

    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
}
