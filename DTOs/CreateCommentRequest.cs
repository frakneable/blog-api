using System.ComponentModel.DataAnnotations;

namespace BlogApi.DTOs;

public record CreateCommentRequest(
    [Required(ErrorMessage = "The comment text is required.")]
    [StringLength(1000, ErrorMessage = "The comment cannot be longer than 1000 characters.")]
    string Text
);