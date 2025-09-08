namespace SyncUpC.Application.UseCases.Events.Dtos;

public record EventImagesDto(
    string Id,
    string EventId,
    List<string> ImageUrls,
    string UploadedByUserId,
    string UploadedByUserName,
    DateTime UploadedAt,
    string Description
);