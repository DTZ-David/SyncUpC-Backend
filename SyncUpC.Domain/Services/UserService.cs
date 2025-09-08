using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Student> _studentRepository;
    private readonly IGenericRepository<AcademicEvent> _eventRepository;

    public UserService(IGenericRepository<User> userRepository, IGenericRepository<Student> studentRepository, IGenericRepository<AcademicEvent> eventRepository)
    {
        _userRepository = userRepository;
        _studentRepository = studentRepository;
        _eventRepository = eventRepository;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await _userRepository.Add(user);
        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = (await _userRepository.FindAsync(
             u => u.Email == email)).FirstOrDefault();

        return user!;
    }

    public async Task<User> GetUserById(string id)
    {
        var user = (await _userRepository.FindAsync(
            u => u.Id == id)).FirstOrDefault();

        return user!;
    }

    public async Task<Dictionary<string, string>> GetUserFaculties(List<string> userIds)
    {
        var students = await _studentRepository.GetAll(); // Task<IEnumerable<Student>>

        return students
            .Where(s => userIds.Contains(s.Id))
            .ToDictionary(
                s => s.Id,
                s => s.Career?.Name ?? "Desconocida"
            );
    }


    public async Task<User> UpdateUser(User user)
    {
        await _userRepository.Update(user);
        return user;
    }


}
