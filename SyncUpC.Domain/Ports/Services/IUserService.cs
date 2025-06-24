using SyncUpC.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Ports.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    Task<User> GetUserByEmail (string email);
}
