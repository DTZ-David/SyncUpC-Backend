using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Domain.Ports.Services;

public interface ICampusService
{
    Task<List<Campus>> GetCampuses();
    Task<Campus> GetCampusById(string id);
}
