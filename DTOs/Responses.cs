namespace BlogApi.Dtos;

// List of posts with comment counts
public record PostSummaryResponse(Guid Id, string Title, int CommentCount);

// Response for the details of a post
public record PostDetailResponse(Guid Id, string Title, string Content, List<CommentResponse> Comments);

// Response for an individual comment
public record CommentResponse(Guid Id, string Text);