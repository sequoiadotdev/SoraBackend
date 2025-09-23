namespace Sora.DTOs;

public class CreateCourseRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Level { get; set; }
}