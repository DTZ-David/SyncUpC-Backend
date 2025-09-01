using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Domain.Ports.Services;

public interface ISpaceService
{
    Task<List<Space>> GetSpaces();
    Task<Space> GetSpaceById(string id);
}
