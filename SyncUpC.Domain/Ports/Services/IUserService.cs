using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Domain.Ports.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserById(string id);
    Task<User> UpdateUser(User user);

}
