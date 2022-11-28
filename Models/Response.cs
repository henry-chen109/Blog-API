namespace BlogApi.Models;

public class Response {
    public int StatusCode { get; set; }
    public string StatusDescription { get; set; } = null!;
    public User? User { get; set; }
    public List<User>? Users { get; set; } 
    public int? PostNum { get; set; }
    public Post? Post { get; set; }
    public List<Post>? Posts { get; set; }
}