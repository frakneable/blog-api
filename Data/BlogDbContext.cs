using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Data;

public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public DbSet<BlogPost> BlogPosts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
}