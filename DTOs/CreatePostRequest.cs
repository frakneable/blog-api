using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos;

public record CreatePostRequest(
    [Required(ErrorMessage = "The title is required.")]
    [StringLength(200, ErrorMessage = "The title cannot be longer than 200 characters.")]
    string Title,

    [Required(ErrorMessage = "The content is required.")]
    string Content
);