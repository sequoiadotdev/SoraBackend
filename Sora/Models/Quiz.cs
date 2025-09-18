namespace Sora.Models;

public class Quiz
{
    public long Id { get; set; }
    public long LessonId { get; set; }
    public string Question { get; set; } = "";
    public string Answer { get; set; } = "";
}