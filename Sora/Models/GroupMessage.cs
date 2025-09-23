namespace Sora.Models;

public class GroupMessage
{
    public long Id { get; set; }

    public long GroupId { get; set; }
    public Group Group { get; set; }

    public long SenderId { get; set; }
    public User Sender { get; set; }

    public string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
