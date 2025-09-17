namespace Sora.Models;

public class Lesson
{
    public long Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public ICollection<VocabularyItem> Vocabulary { get; set; } = new List<VocabularyItem>();
}