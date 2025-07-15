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
}
