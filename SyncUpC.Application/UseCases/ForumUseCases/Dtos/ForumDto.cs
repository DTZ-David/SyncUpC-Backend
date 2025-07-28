namespace SyncUpC.Application.UseCases.ForumUseCases.Dtos;

public record ForumDto
    (
        string Id,
        string EventId,
        string AuthorName,
        string AuthorId,
        string AuthorProfilePicture,
        string Title,
        string Content,
        List<CommentDto> Comments,
        string Time

    );
