using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class EventTypeService : IEventTypeService
{
    private readonly IGenericRepository<EventType> _categoryRepository;

    public EventTypeService(IGenericRepository<EventType> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<EventType>> GetAllEventTypes()
    {
        var types = await _categoryRepository.GetAll();
        return types.ToList();
    }

    public async Task<EventType> GetEventType(string id)
    {
        var types = await _categoryRepository.GetById(id);
        return types;
    }
}
