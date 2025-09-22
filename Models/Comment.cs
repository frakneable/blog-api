using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApi.Models;

public class Comment
{
    public Guid Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    public Guid BlogPostId { get; set; }

    [JsonIgnore]
    public BlogPost? BlogPost { get; set; }
}