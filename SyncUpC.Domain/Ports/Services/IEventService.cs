using SyncUpC.Domain.Entities.Events;


namespace SyncUpC.Domain.Ports.Services;

public interface IEventService
{
    Task<AcademicEvent> CreateEventAsync(AcademicEvent academicEvent);
    Task<List<AcademicEvent>> GetAllEvents();
    Task<List<AcademicEvent>> GetAllEventsForMetrics();
    Task<AcademicEvent> UpdateEvent(AcademicEvent academicEvent);
    Task<AcademicEvent> GetEventById(string id);
    Task<List<AcademicEvent>> GetSavedEvents(List<string> eventIds);
    Task<List<AcademicEvent>> GetEventsForU(string careerId);
    Task<AcademicEvent> DeleteEvent(string id);
    Task<List<AcademicEvent>> GetEventsMadeForU(string userId);

    Task<List<AcademicEvent>> GetEventsFilteredAsync(
       DateTime? dateFrom,
       DateTime? dateTo,
       string? faculty,
       string? program,
       string? eventType,
       string? category);
}