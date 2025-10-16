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
        return ev!;
    }

    public async Task<List<AcademicEvent>> GetAllEvents()
    {
        var events = await _eventRepository.FindAsync(x => x.Status != "completed");
        await EnsureEventsStatusAreUpToDate(events);
        return events.ToList();
    }

    public async Task<List<AcademicEvent>> GetEventsForU(string careerId)
    {
        var events = await _eventRepository.FindAsync(e =>
            e.Careers.Any(c => c.Id == careerId) && e.Status != "completed");

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
        if (ev.Status == "completed") return; // ya está actualizado

        if (ev.EndDate < DateTime.UtcNow)
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

    public async Task<List<AcademicEvent>> GetAllEventsForMetrics()
    {
        var events = await _eventRepository.GetAll();
        return events.ToList();
    }

    public async Task<List<AcademicEvent>> GetEventsFilteredAsync(DateTime? dateFrom, DateTime? dateTo, string? faculty, string? program, string? eventType, string? category)
    {
        var events = await _eventRepository.GetAll(); // devuelve IEnumerable<AcademicEvent>
        var query = events.AsQueryable();

        if (dateFrom.HasValue)
            query = query.Where(e => e.StartDate >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(e => e.StartDate <= dateTo.Value);

        if (!string.IsNullOrEmpty(program))
            query = query.Where(e => e.Faculties != null && e.Faculties.Any(c => c.Id == faculty));

        if (!string.IsNullOrEmpty(program))
            query = query.Where(e => e.Careers != null && e.Careers.Any(c => c.Id == program));

        if (!string.IsNullOrEmpty(eventType))
            query = query.Where(e => e.EventTypes != null && e.EventTypes.Any(et => et.Id == eventType));

        if (!string.IsNullOrEmpty(category))
            query = query.Where(e => e.EventTypes != null && e.EventTypes.Any(et => et.Id == category));

        return query.ToList();
    }
}
