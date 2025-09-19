namespace Sora.Models;

public class Lesson
{
    public long Id { get; set; }
    public long CourseId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<VocabularyItem> Vocabulary { get; set; } = new List<VocabularyItem>();
}