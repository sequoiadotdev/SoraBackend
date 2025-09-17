namespace Sora.Models;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = ""; // NOT plaintext!
    public string Email { get; set; } = "";
    public ICollection<Progress> ProgressRecords { get; set; } = new List<Progress>();
}