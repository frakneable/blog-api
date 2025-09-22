using BlogApi.Data;
using BlogApi.Dtos;
using BlogApi.DTOs;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers;

public static class PostController
{
    public static void MapPostsEndpoints(this WebApplication app)
    {
        RouteGroupBuilder postsGroup = app.MapGroup("/api/posts").WithTags("Posts");

        // GET /api/posts: List all posts
        postsGroup.MapGet("/", async (BlogDbContext db) =>
        {
            List<PostSummaryResponse> posts = await db.BlogPosts
                .Select(p => new PostSummaryResponse(p.Id, p.Title, p.Comments.Count))
                .AsNoTracking()
                .ToListAsync();
            return Results.Ok(posts);
        });

        // GET /api/posts/{id}: Details of a specific post
        postsGroup.MapGet("/{id:guid}", async (Guid id, BlogDbContext db) =>
        {
            BlogPost? post = await db.BlogPosts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post is null)
            {
                return Results.NotFound($"Post with ID {id} not found.");
            }

            PostDetailResponse response = new(
                post.Id,
                post.Title,
                post.Content,
                post.Comments.Select(c => new CommentResponse(c.Id, c.Text)).ToList()
            );
            return Results.Ok(response);
        });

        // POST /api/posts: Create a new post
        postsGroup.MapPost("/", async (CreatePostRequest request, BlogDbContext db) =>
        {
            BlogPost newPost = new()
            {
                Title = request.Title,
                Content = request.Content
            };

            await db.BlogPosts.AddAsync(newPost);
            await db.SaveChangesAsync();

            PostSummaryResponse response = new(newPost.Id, newPost.Title, 0);
            return Results.Created($"/api/posts/{newPost.Id}", response);
        });

        // POST /api/posts/{id}/comments: Add a comment to a specific post
        postsGroup.MapPost("/{id:guid}/comments", async (Guid id, CreateCommentRequest request, BlogDbContext db) =>
        {
            BlogPost? post = await db.BlogPosts.FindAsync(id);
            if (post is null)
            {
                return Results.NotFound($"Post with ID {id} not found.");
            }

            Comment newComment = new()
            {
                Text = request.Text,
                BlogPostId = id
            };

            await db.Comments.AddAsync(newComment);
            await db.SaveChangesAsync();

            CommentResponse response = new(newComment.Id, newComment.Text);
            return Results.Created($"/api/posts/{id}/comments/{newComment.Id}", response);
        });
    }
}