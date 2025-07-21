using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class CareerService : ICareerService
{
    private readonly IGenericRepository<Faculty> _facultyRepository;

    public CareerService(IGenericRepository<Faculty> facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }

    // ✅ Devuelve todas las carreras de todas las facultades
    public async Task<List<Career>> GetAll()
    {
        var faculties = await _facultyRepository.GetAll();
        return faculties.SelectMany(f => f.Careers).ToList();
    }

    // ✅ Devuelve las carreras de una facultad específica
    public async Task<List<Career>> GetByFaculty(string facultyId)
    {
        var faculty = await _facultyRepository.GetById(facultyId);
        return faculty?.Careers ?? new List<Career>();
    }

    // ✅ Busca una carrera por su ID en todas las facultades
    public async Task<Career> GetCareerById(string id)
    {
        var faculties = await _facultyRepository.GetAll();

        var career = faculties
            .SelectMany(f => f.Careers)
            .FirstOrDefault(c => c.Id == id);

        return career ?? throw new Exception("Career not found");
    }
}
