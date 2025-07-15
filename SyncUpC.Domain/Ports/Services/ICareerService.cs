using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Domain.Ports.Services
{
    public interface ICareerService
    {
        Task<Career> GetCareerById(string id);
        Task<List<Career>> GetAll();
        Task<List<Career>> GetByFaculty(string facultyId);
    }
}
