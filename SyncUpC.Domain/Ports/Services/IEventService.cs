using SyncUpC.Domain.Entities.Events;


namespace SyncUpC.Domain.Ports.Services;

public interface IEventService
{
    Task<AcademicEvent> CreateEventAsync(AcademicEvent academicEvent);
    Task<List<AcademicEvent>> GetAllEvents();
    Task<AcademicEvent> UpdateEvent(AcademicEvent academicEvent);
    Task<AcademicEvent> GetEventById(string id);
    Task<List<AcademicEvent>> GetSavedEvents(List<string> eventIds);
    Task<List<AcademicEvent>> GetEventsForU(string careerId);
    Task<AcademicEvent> DeleteEvent(string id);

}