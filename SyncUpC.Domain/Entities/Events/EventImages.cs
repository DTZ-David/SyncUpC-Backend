using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.Events;

public class EventImages : BaseEntity<string>
{
    public EventImages(
        string eventId,
        List<string> imageUrls,
        string uploadedByUserId,
        string uploadedByUserName,
        DateTime eventDate,
        string eventTitle
    )
    {
        EventId = eventId;
        ImageUrls = imageUrls ?? new List<string>();
        UploadedByUserId = uploadedByUserId;
        UploadedByUserName = uploadedByUserName;
        EventDate = eventDate;
        EventTitle = eventTitle;
        IsActive = true;
    }

    public string EventId { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public string UploadedByUserId { get; set; }
    public string UploadedByUserName { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}