using Microsoft.EntityFrameworkCore;
namespace BlogApi.Models;
public class BlogApiContext : DbContext {
    protected readonly IConfiguration Configuration;

    public BlogApiContext(DbContextOptions<BlogApiContext> options,  IConfiguration configuration) : base(options) {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        string connectionString = Configuration.GetConnectionString("BlogApiDatabase");
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
}

    