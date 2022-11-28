namespace BlogApi.Models;
public class Post {
    public int PostId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public int? UserId { get; set; }
}