using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class EventCategoryService : IEventCategoryService
{
    private readonly IGenericRepository<EventCategory> _categoryRepository;

    public EventCategoryService(IGenericRepository<EventCategory> userRepository)
    {
        _categoryRepository = userRepository;
    }

    public async Task<List<EventCategory>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAll();
        return categories.ToList();
    }

    public async Task<EventCategory> GetCategoryById(string id)
    {
        var categories = await _categoryRepository.GetById(id);
        return categories;
    }
}
