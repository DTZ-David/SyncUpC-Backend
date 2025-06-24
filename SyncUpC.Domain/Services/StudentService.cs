using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;



namespace SyncUpC.Domain.Services;

[ApplicationService]
public class StudentService : IUserService
{
    private readonly IGenericRepository<User> _studentRepository;


    public StudentService(IGenericRepository<User> studentRepository)
    {
        _studentRepository = studentRepository;
      
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await _studentRepository.Add(user);
        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = (await _studentRepository.FindAsync(
             u => u.Email == email)).FirstOrDefault();

        return user!;
    }
}
