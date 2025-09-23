namespace Sora.Models;

public class GroupMember
{
    public long Id { get; set; }

    public long GroupId { get; set; }
    public Group Group { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public GroupRole Role { get; set; } = GroupRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}

public enum GroupRole
{
    Member,
    Moderator,
    Admin
}
