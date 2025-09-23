using BlogApi.Data;
using BlogApi.Dtos;
using BlogApi.DTOs;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BlogApi.Controllers;

public static class PostsEndpoints
{
    public static void MapPostsEndpoints(this WebApplication app)
    {
        var postsGroup = app.MapGroup("/api/posts").WithTags("Posts");

        // GET all posts
        postsGroup.MapGet("/", async (BlogDbContext db) =>
        {
            var posts = await db.BlogPosts
                .Select(p => new PostSummaryResponse(p.Id, p.Title, p.Comments.Count))
                .AsNoTracking()
                .ToListAsync();
            return Results.Ok(posts);
        })
        .WithSummary("Retrieves a summary list of all blog posts.")
        .Produces<List<PostSummaryResponse>>(StatusCodes.Status200OK);

        // GET post by ID
        postsGroup.MapGet("/{id:guid}", async (Guid id, BlogDbContext db) =>
        {
            var post = await db.BlogPosts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post is null)
            {
                return Results.NotFound($"Post with ID {id} not found.");
            }

            var response = new PostDetailResponse(
                post.Id,
                post.Title,
                post.Content,
                post.Comments.Select(c => new CommentResponse(c.Id, c.Text)).ToList()
            );
            return Results.Ok(response);
        })
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Retrieves a specific blog post by its unique ID.",
        })
        .Produces<PostDetailResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // POST a new blog post
        postsGroup.MapPost("/", async (CreatePostRequest request, BlogDbContext db) =>
        {
            var newPost = new BlogPost
            {
                Title = request.Title,
                Content = request.Content
            };

            await db.BlogPosts.AddAsync(newPost);
            await db.SaveChangesAsync();

            var response = new PostSummaryResponse(newPost.Id, newPost.Title, 0);
            return Results.Created($"/api/posts/{newPost.Id}", response);
        })
        .WithSummary("Creates a new blog post.")
        .Produces<PostSummaryResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        // POST a new comment to a blog post
        postsGroup.MapPost("/{id:guid}/comments", async (Guid id, CreateCommentRequest request, BlogDbContext db) =>
        {
            var post = await db.BlogPosts.FindAsync(id);
            if (post is null)
            {
                return Results.NotFound($"Post with ID {id} not found.");
            }

            var newComment = new Comment
            {
                Text = request.Text,
                BlogPostId = id
            };

            await db.Comments.AddAsync(newComment);
            await db.SaveChangesAsync();

            var response = new CommentResponse(newComment.Id, newComment.Text);
            return Results.Created($"/api/posts/{id}/comments/{newComment.Id}", response);
        })
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Adds a new comment to a specific blog post."
        })
        .Produces<CommentResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);
    }
}