namespace Sora.Models;

// TODO: Make private groups work (and maybe assignments?)
public class Group
{
    public long Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public long CreatedById { get; set; }
    public bool IsPrivate { get; set; } = false;
    public User CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GroupMember> Members { get; set; }
    public ICollection<GroupMessage> Messages { get; set; }
}
