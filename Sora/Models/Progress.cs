namespace Sora.Models;

public class Progress
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long LessonId { get; set; }
    public bool Completed { get; set; }
    public DateTime LastStudied { get; set; }
}