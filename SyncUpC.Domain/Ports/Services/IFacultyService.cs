using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Domain.Ports.Services;

public interface IFacultyService
{
    Task<Faculty> GetFacultyById(string id);
    Task<List<Faculty>> GetAllFacultyAsync();
}
