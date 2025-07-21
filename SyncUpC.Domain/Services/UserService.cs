using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;



namespace SyncUpC.Domain.Services;

[ApplicationService]
public class UserService : IUserService
{
    private readonly IGenericRepository<User> _studentRepository;
    private readonly IGenericRepository<AcademicEvent> _eventRepository;

    public UserService(IGenericRepository<User> studentRepository, IGenericRepository<AcademicEvent> eventRepository)
    {
        _studentRepository = studentRepository;
        _eventRepository = eventRepository;
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

    public async Task<User> GetUserById(string id)
    {
        var user = (await _studentRepository.FindAsync(
            u => u.Id == id)).FirstOrDefault();

        return user!;
    }

    public async Task<User> UpdateUser(User user)
    {
        await _studentRepository.Update(user);
        return user;
    }


}
