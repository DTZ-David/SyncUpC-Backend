using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;



namespace SyncUpC.Domain.Services;

[ApplicationService]
public class StudentService : IStudentService
{
    private readonly IGenericRepository<Student> _studentRepository;


    public StudentService(IGenericRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
      
    }

    public async Task<Student> CreateStudentAsync(Student user)
    {
        await _studentRepository.Add(user);
        return user;
    }

    public async Task<Student> GetStudentByEmail(string email)
    {
        var user = (await _studentRepository.FindAsync(
             u => u.Email == email)).FirstOrDefault();

        return user!;
    }
}
