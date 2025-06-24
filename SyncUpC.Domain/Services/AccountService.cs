using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;


[ApplicationService]
public class AccountService : IAccountService
{
    public Task<string> ValidateMobileApp(string email, string password)
    {
        throw new NotImplementedException();
    }
}
