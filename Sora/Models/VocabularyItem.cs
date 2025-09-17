namespace Sora.Models;

public class VocabularyItem
{
    public long Id { get; set; }
    public long LessonId { get; set; }
    public string Japanese { get; set; } = "";
    public string Romaji { get; set; } = "";
    public string English { get; set; } = "";
    public string ExampleSentence { get; set; } = "";
}