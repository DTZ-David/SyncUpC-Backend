using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Domain.Ports.Services;

public interface IEventTypeService
{
    Task<List<EventType>> GetAllEventTypes();
    Task<EventType> GetEventType(string id);
}
