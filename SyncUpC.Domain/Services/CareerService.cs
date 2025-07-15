using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class CareerService : ICareerService
{
    private readonly IGenericRepository<Career> _careerRepository;

    public CareerService(IGenericRepository<Career> careerRepository)
    {
        _careerRepository = careerRepository;
    }

    public async Task<List<Career>> GetAll()
    {
        var careers = await _careerRepository.GetAll();
        return careers.ToList();
    }

    public async Task<List<Career>> GetByFaculty(string facultyId)
    {
        var careers = await _careerRepository.FindAsync(c => c.FacultyId == facultyId);
        return careers.ToList();
    }

    public async Task<Career> GetCareerById(string id)
    {
        var career = (await _careerRepository.FindAsync(c => c.Id == id)).FirstOrDefault();
        return career ?? throw new Exception("Career not found");
    }
}
