using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class SpaceService : ISpaceService
{
    private readonly IGenericRepository<Space> _categoryRepository;

    public SpaceService(IGenericRepository<Space> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Space> GetSpaceById(string id)
    {
        var spaces = await _categoryRepository.GetById(id);
        return spaces;
    }

    public async Task<List<Space>> GetSpaces()
    {
        var spaces = await _categoryRepository.GetAll();
        return spaces.ToList();
    }
}
