using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Domain.Ports.Services;

public interface IEventImageService
{
    Task<EventImages> CreateEventImages(EventImages eventImages);
    Task<List<EventImages>> GetEventImagesByEventId(string eventId);
    Task<List<EventImages>> GetEventImagesByUserId(string userId);
    Task<EventImages?> GetEventImagesById(string id);
    Task DeleteEventImages(string id);
    Task<List<EventImages>> GetAllEventImages();
}
