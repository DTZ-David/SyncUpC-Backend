
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Configuration.JsonWebToken;
using SyncUpC.Domain.Ports.Configuration.Localization;
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
