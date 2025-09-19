namespace Sora.Models;

public class Course
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Level { get; set; } = "";
    public string Language { get; set; } = "Japanese";
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;
    public bool IsPublished { get; set; } = false;
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}