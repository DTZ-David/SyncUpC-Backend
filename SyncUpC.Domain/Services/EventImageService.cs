using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class EventImageService : IEventImageService
{
    private readonly IGenericRepository<EventImages> _eventImagesRepository;

    public EventImageService(IGenericRepository<EventImages> eventImagesRepository)
    {
        _eventImagesRepository = eventImagesRepository;
    }

    public async Task<EventImages> CreateEventImages(EventImages eventImages)
    {
        await _eventImagesRepository.Add(eventImages);
        return eventImages;
    }

    public async Task DeleteEventImages(string id)
    {
        var eventImages = await _eventImagesRepository.GetById(id);
        if (eventImages != null)
        {
            await _eventImagesRepository.Delete(eventImages);
        }
    }

    public async Task<List<EventImages>> GetAllEventImages()
    {
        var eventImages = await _eventImagesRepository.GetAll();
        return eventImages.Where(ei => ei.IsActive).ToList();
    }

    public async Task<List<EventImages>> GetEventImagesByEventId(string eventId)
    {
        var allEventImages = await _eventImagesRepository.GetAll();
        return allEventImages
            .Where(ei => ei.EventId == eventId && ei.IsActive)
            .OrderByDescending(ei => ei.UploadedAt)
            .ToList();
    }

    public async Task<EventImages?> GetEventImagesById(string id)
    {
        var eventImages = await _eventImagesRepository.GetById(id);
        return eventImages?.IsActive == true ? eventImages : null;
    }

    public async Task<List<EventImages>> GetEventImagesByUserId(string userId)
    {
        var allEventImages = await _eventImagesRepository.GetAll();
        return allEventImages
            .Where(ei => ei.UploadedByUserId == userId && ei.IsActive)
            .OrderByDescending(ei => ei.UploadedAt)
            .ToList();
    }
}