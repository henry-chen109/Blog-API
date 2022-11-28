using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase {
    private readonly BlogApiContext _context;
    private Response PostResponse;

    public PostController(BlogApiContext context) {
        _context = context;
        PostResponse = new Response();
    }

    // GET api/post
    [HttpGet]
    public async Task<Response> GetPosts() {
        var postList = await _context.Posts.ToListAsync();
        
        if (postList == null) {
            this.PostResponse.StatusCode = 404;
            this.PostResponse.StatusDescription = "Posts not found";
        } else {
            this.PostResponse.StatusCode = 200;
            this.PostResponse.Posts = postList;
            this.PostResponse.StatusDescription = "Posts found";
        }

        return this.PostResponse;
    }

    // GET api/post/1
    [HttpGet("{id}")]
    public async Task<Response> GetPost(int id) {
        var post = await _context.Posts.FindAsync(id);

        if (post == null || id == 0) {
            this.PostResponse.StatusCode = 404;
            this.PostResponse.StatusDescription = $"Post of id {id} not found";
            return this.PostResponse;
        }

        this.PostResponse.StatusCode = 200;
        this.PostResponse.StatusDescription = $"Post of id {id} found";
        this.PostResponse.Post = post;

        return this.PostResponse;
    }

    // DELETE api/post/1
    [HttpDelete("{id}")]
    public async Task<Response> DeletePost(int id) {
        var post = await _context.Posts.FindAsync(id);

        if (post == null) {
            this.PostResponse.StatusCode = 404;
            this.PostResponse.StatusDescription = $"Post of id {id} could not be found";
            return this.PostResponse;
        } 

        _context.Posts.Remove(post);

        try {
            await _context.SaveChangesAsync();
            this.PostResponse.StatusCode = 204;
            this.PostResponse.StatusDescription = $"Post of id {id} was removed";
        } catch {
            this.PostResponse.StatusCode = 400;
            this.PostResponse.StatusDescription = $"Post of id {id} could not be removed";
        }

        return this.PostResponse;
        
    }
    
    private bool PostExists(int id) {
        return _context.Posts.Any(e => e.PostId == id);
    }

}