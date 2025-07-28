namespace SyncUpC.Application.UseCases.ForumUseCases.Dtos;

public record CommentDto(
    string ForumId,
    string AuthorId,
    string AuthorName,
    string AuthorProfilePicture,
    string Content,
    string Time
    );


