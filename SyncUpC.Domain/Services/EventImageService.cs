using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class EventImageService : IEventImageService
{
    private readonly IGenericRepository<EventImages> _eventImagesRepository;
    private readonly IGenericRepository<AcademicEvent> _eventRepository;

    public EventImageService(IGenericRepository<EventImages> eventImagesRepository, IGenericRepository<AcademicEvent> eventRepository)
    {
        _eventImagesRepository = eventImagesRepository;
        _eventRepository = eventRepository;
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
            .OrderByDescending(ei => ei.EventDate)
            .ToList();
    }

    public async Task<EventImages?> GetEventImagesById(string id)
    {
        var eventImages = await _eventImagesRepository.GetById(id);
        return eventImages?.IsActive == true ? eventImages : null;
    }

    public async Task<List<EventImages>> GetEventImagesByUserId(string userId)
    {
        // Obtener todos los eventos académicos del usuario que están completados
        var completedEvents = await _eventRepository.GetAll();
        var userCompletedEvents = completedEvents
            .Where(ae => ae.Organizer.Id == userId && ae.Status == "completed") // Ajusta el status según tu aplicación
            .ToList();

        // Transformar AcademicEvent a EventImages
        return userCompletedEvents
            .Select(ae => new EventImages(
                eventId: ae.Id,
                imageUrls: ae.ImageUrls,
                uploadedByUserId: ae.Organizer.Id,
                uploadedByUserName: ae.Organizer.Name, // Asumiendo que Organizer tiene una propiedad Name
                eventDate: ae.StartDate,
                eventTitle: ae.EventTitle
            ))
            .OrderByDescending(ei => ei.EventDate)
            .ToList();
    }
}