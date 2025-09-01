using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Domain.Ports.Services
{
    public interface IEventCategoryService
    {
        Task<List<EventCategory>> GetAllCategories();

        Task<EventCategory> GetCategoryById(string id);
    }
}
