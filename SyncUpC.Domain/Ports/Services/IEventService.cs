using SyncUpC.Domain.Entities.Events;


namespace SyncUpC.Domain.Ports.Services;

public interface IEventService
{
    Task<AcademicEvent> CreateEventAsync(AcademicEvent academicEvent);

    Task<List<AcademicEvent>> GetAllEvents();

    Task<AcademicEvent> GetEventById(string eventId);

}