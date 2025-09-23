namespace Sora.Models;

public class FriendRequest
{
    public long Id { get; set; }

    public long FromUserId { get; set; }
    public User FromUser { get; set; }

    public long ToUserId { get; set; }
    public User ToUser { get; set; }

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;
}

public enum FriendRequestStatus
{
    Pending,
    Accepted,
    Rejected
}
