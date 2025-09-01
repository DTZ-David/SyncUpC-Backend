using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class EventService : IEventService
{
    private readonly IGenericRepository<AcademicEvent> _eventRepository;

    public EventService(IGenericRepository<AcademicEvent> eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<AcademicEvent> CreateEventAsync(AcademicEvent academicEvent)
    {
        await _eventRepository.Add(academicEvent);
        return academicEvent;
    }

    public async Task<AcademicEvent> DeleteEvent(string id)
    {
        var academicEvent = await _eventRepository.GetById(id);
        await _eventRepository.Delete(academicEvent);
        return academicEvent;
    }

    public async Task<AcademicEvent> GetEventById(string eventId)
    {
        var ev = await _eventRepository.GetById(eventId);
        if (ev != null)
            await EnsureEventStatusIsUpToDate(ev);
        return ev;
    }

    public async Task<List<AcademicEvent>> GetAllEvents()
    {
        var events = await _eventRepository.GetAll();
        await EnsureEventsStatusAreUpToDate(events);
        return events.ToList();
    }

    public async Task<List<AcademicEvent>> GetEventsForU(string careerId)
    {
        var events = await _eventRepository.FindAsync(e =>
            e.Careers.Any(c => c.Id == careerId));

        await EnsureEventsStatusAreUpToDate(events);
        return events.ToList();
    }

    public async Task<List<AcademicEvent>> GetEventsMadeForU(string userId)
    {
        var events = await _eventRepository.FindAsync(e =>
            e.Organizer.Id == userId);

        await EnsureEventsStatusAreUpToDate(events);
        return events.ToList();
    }

    public async Task<List<AcademicEvent>> GetSavedEvents(List<string> eventIds)
    {
        var events = await _eventRepository.FindAsync(e => eventIds.Contains(e.Id));
        await EnsureEventsStatusAreUpToDate(events);
        return events.ToList();
    }

    public async Task<AcademicEvent> UpdateEvent(AcademicEvent academicEvent)
    {
        await _eventRepository.Update(academicEvent);
        return academicEvent;
    }

    // --- Métodos privados reutilizables ---
    private async Task EnsureEventStatusIsUpToDate(AcademicEvent ev)
    {
        if (ev.EndDate < DateTime.UtcNow && ev.Status != "completed")
        {
            ev.Status = "completed";
            await _eventRepository.Update(ev);
        }
    }

    private async Task EnsureEventsStatusAreUpToDate(IEnumerable<AcademicEvent> events)
    {
        foreach (var ev in events)
        {
            await EnsureEventStatusIsUpToDate(ev);
        }
    }
}
