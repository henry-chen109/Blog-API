using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {
    protected readonly BlogApiContext _context;
    private Response UserResponse;

    public UserController(BlogApiContext context) {
        _context = context;
        UserResponse = new Response();
    }
    
    // GET api/user
    [HttpGet]
    public async Task<Response> GetUsers() {
        var userList = await _context.Users.ToListAsync();
        var postList = await _context.Posts.ToListAsync();
        
        if (userList == null) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = "Users not found";
        } else {
            this.UserResponse.StatusCode = 200;
            this.UserResponse.Users = userList;
            if (userList.Count() == 0) {
                this.UserResponse.StatusDescription = "Succeeded in getting Users, however there are currently no users in the database";
            } else {
                this.UserResponse.StatusDescription = "Users found";
            }
        }

        return this.UserResponse;
    }

    // GET api/user/1
    [HttpGet("{id}")]
    public async Task<Response> GetUser(int id) {
        var user = await _context.Users.FindAsync(id);
        var posts = await _context.Posts.ToListAsync();
        
        if (id == 0 || user == null) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = $"No user of id {id} exists";
            return this.UserResponse;
        } 

        this.UserResponse.StatusCode = 200;
        this.UserResponse.StatusDescription = $"User id {id} found";
        this.UserResponse.User = user;
        
        return this.UserResponse;
    }

    // GET api/user/1/post/1
    [HttpGet("{userId}/post/{postNum}")]
    public async Task<Response> GetUserPost(int userId, int postNum) {
        var user = await _context.Users.Where(user => user.UserId == userId).FirstOrDefaultAsync();
        var post = await _context.Posts.Where(post => post.UserId == userId).OrderBy(p => p.DateCreated).Skip(postNum - 1).FirstOrDefaultAsync();
    
        if (user == null) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = "User not found";
        } else if (post == null) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = "Post not found";
        } else {
            this.UserResponse.StatusCode = 200;
            this.UserResponse.User = user;
            this.UserResponse.StatusDescription = "User found";
        }

        return this.UserResponse;
    }

    // PUT api/user/1
    [HttpPut("{id}")]
    public async Task<Response> PutUser(int id, User user) {
        if (user.DateJoined != default(DateTime) || user.Username != null || user.UserId != default(int)) {
            this.UserResponse.StatusCode = 400;
            this.UserResponse.StatusDescription = "The values entered cannot be modified.";
            return this.UserResponse;
        }

        var currUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);

        if (currUser != null) {
            user.Username = currUser.Username;
            user.DateJoined = currUser.DateJoined;
            user.UserId = currUser.UserId;

            if (user.Email == null) {
                user.Email = currUser.Email;
            }

            if (user.AboutUser == null) {
                user.AboutUser = currUser.AboutUser;
            }
        }

        _context.Entry(user).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!UserExists(id)) {
                this.UserResponse.StatusCode = 404;
                this.UserResponse.StatusDescription = $"Cannot update user with id of {id}; User does not exist";
                return this.UserResponse;
            }
        }

        this.UserResponse.StatusCode = 204;
        this.UserResponse.StatusDescription = "Update has succeeded";

        return this.UserResponse;
    }

    // PUT api/user/1/post/1
    [HttpPut("{userId}/post/{postNum}")]
    public async Task<Response> PutUserPost(int userId, int postNum, Post post) {
        var user = await _context.Users.FindAsync(userId);

        if ( user == null ) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = $"User of id {userId} does not exist";
            return this.UserResponse;
        }

        var currPost = await _context.Posts.AsNoTracking().Where(p => p.UserId == userId).OrderBy(r => r.DateCreated).Skip(postNum - 1).FirstOrDefaultAsync();
            
        if (currPost == null) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = $"Post number {postNum} does not exist for User of id {userId}";
            return this.UserResponse;
        }

        if (post.DateCreated != default(DateTime) || post.DateUpdated != null || post.PostId != default(int) || post.UserId != null) {
            this.UserResponse.StatusCode = 400;
            this.UserResponse.StatusDescription = "Field(s) you have supplied cannot be modified";
            return this.UserResponse;
        }

        post.PostId = currPost.PostId;
        post.DateUpdated = DateTime.Now;
        post.DateCreated = currPost.DateCreated;
        post.UserId = currPost.UserId;

        if (post.Content == null) {
            post.Content = currPost.Content;
        }

        if(post.Title == null) {
            post.Title = currPost.Title;
        }

        _context.Entry(post).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
            this.UserResponse.StatusCode = 204;
            this.UserResponse.StatusDescription = $"Updated post {postNum} for User {userId}";
        } catch(DbUpdateConcurrencyException) {
            this.UserResponse.StatusCode = 400;
            this.UserResponse.StatusDescription = "Cannot update post";
        }

        return this.UserResponse;
    }

    // POST api/user
    [HttpPost]
    public async Task<Response> PostUser(User user) {
        user.DateJoined = DateTime.Now;
        _context.Users.Add(user);
        try {
            await _context.SaveChangesAsync();
            this.UserResponse.StatusCode = 201;
            this.UserResponse.StatusDescription = $"User {user.Username} has been successfully created";
        } catch {
            this.UserResponse.StatusCode = 400;
            this.UserResponse.StatusDescription = $"Could not create user {user.Username}";
        }

        return this.UserResponse;
    }

    // POST api/user/1/post
    [HttpPost("{id}/post")]
    public async Task<Response> PostToUser(int id, Post post) {
        var user = await _context.Users.FindAsync(id);

        if (user == null) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = $"User of id {id} not found";
            return this.UserResponse;
        }

        if (post.DateUpdated != null) {
            this.UserResponse.StatusCode = 400;
            this.UserResponse.StatusDescription = "Values you have entered cannot be changed";
            return this.UserResponse;
        }

        post.UserId = id;
        post.DateCreated = DateTime.Now;
        _context.Posts.Add(post);

        try {
            await _context.SaveChangesAsync();
            this.UserResponse.StatusCode = 201;
            this.UserResponse.StatusDescription = $"Post for user of id {id} created";
        } catch {
            this.UserResponse.StatusCode = 400;
            this.UserResponse.StatusDescription = $"Cannot create post for user of id {id}";
        }

        return this.UserResponse;
    }

    // DELETE api/user/1
    [HttpDelete("{id}")]
    public async Task<Response> DeleteUser(int id) {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = $"User of id {id} not found";
            return this.UserResponse;
        }

        _context.Users.Remove(user);

        try {
            await _context.SaveChangesAsync();
            this.UserResponse.StatusCode = 204;
            this.UserResponse.StatusDescription = $"User of id {id} successfully deleted";
        } catch {
            this.UserResponse.StatusCode = 400;
            this.UserResponse.StatusDescription = $"Could not delete user of id {id}";
        }

        return this.UserResponse;
    }
    
    // DELETE api/user/1/post/1
    [HttpDelete("{id}/post/{postNum}")]
    public async Task<Response> DeletePost(int id, int postNum) {
        var user = await _context.Users.FindAsync(id);

        if (user == null) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = $"User of id {id} could not be found";
            return this.UserResponse;
        }

        var post = await _context.Posts.Where(p => p.UserId == id).OrderBy(r => r.DateCreated).Skip(postNum - 1).FirstOrDefaultAsync();

        if (post == null) {
            this.UserResponse.StatusCode = 404;
            this.UserResponse.StatusDescription = $"Post does not exist";
            return this.UserResponse;
        }

        _context.Posts.Remove(post);

        try {
            await _context.SaveChangesAsync();
            this.UserResponse.StatusCode = 204;
            this.UserResponse.StatusDescription = $"Post {postNum} successfully deleted from user of id {id}";
        } catch {
            this.UserResponse.StatusCode = 400;
            this.UserResponse.StatusDescription = "Could not delete post";
        }

        return this.UserResponse;
    }
    
    private bool UserExists(int id) {
        return _context.Users.Any(e => e.UserId == id);
    }

}
