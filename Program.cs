using BlogApi.Controllers;
using BlogApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.OpenApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add this configuration block
    options.SwaggerDoc("v1", new() { Title = "Blog API", Version = "v1" });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map the endpoints defined in the PostsEndpoints class
app.MapPostsEndpoints();

// Apply EF Core migrations on startup
// Improvement: Consider a more robust migration strategy.
using (IServiceScope scope = app.Services.CreateScope())
{
    BlogDbContext dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    dbContext.Database.Migrate();
}

app.Run();