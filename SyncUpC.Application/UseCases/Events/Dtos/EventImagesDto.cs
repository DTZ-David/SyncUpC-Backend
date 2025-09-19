namespace SyncUpC.Application.UseCases.Events.Dtos;

public record EventImagesDto(
    string Id,
    string EventId,
    string EventTitle,
    DateTime EventDate,
    List<string> ImageUrls,
    string UploadedByUserId,
    string UploadedByUserName
);