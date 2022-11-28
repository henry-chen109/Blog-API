namespace BlogApi.Models;
public class User {
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public DateTime DateJoined { get; set; }
    public string? AboutUser { get; set; }
    public List<Post>? Posts { get; set; }
}