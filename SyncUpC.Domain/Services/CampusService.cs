using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class CampusService : ICampusService
{
    private readonly IGenericRepository<Campus> _categoryRepository;

    public CampusService(IGenericRepository<Campus> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Campus> GetCampusById(string id)
    {
        var campuses = await _categoryRepository.GetById(id);
        return campuses;
    }

    public async Task<List<Campus>> GetCampuses()
    {
        var campuses = await _categoryRepository.GetAll();
        return campuses.ToList();
    }
}

