using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.Events;

public class EventImages : BaseEntity<string>
{
    public EventImages(
        string eventId,
        List<string> imageUrls,
        string uploadedByUserId,
        string uploadedByUserName,
        DateTime uploadedAt,
        string? description = null
    )
    {
        EventId = eventId;
        ImageUrls = imageUrls ?? new List<string>();
        UploadedByUserId = uploadedByUserId;
        UploadedByUserName = uploadedByUserName;
        UploadedAt = uploadedAt;
        Description = description ?? string.Empty;
        IsActive = true;
    }

    public string EventId { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public string UploadedByUserId { get; set; }
    public string UploadedByUserName { get; set; }
    public DateTime UploadedAt { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}