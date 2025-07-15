using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class FacultyService : IFacultyService
{
    private readonly IGenericRepository<Faculty> _facultyRepository;

    public FacultyService(IGenericRepository<Faculty> facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }

    public async Task<List<Faculty>> GetAllFacultyAsync()
    {
        var faculties = await _facultyRepository.GetAll();
        return faculties.ToList();
    }

    public async Task<Faculty> GetFacultyById(string id)
    {
        var faculty = (await _facultyRepository.FindAsync(f => f.Id == id)).FirstOrDefault();
        return faculty ?? throw new Exception("Faculty not found");
    }
}
