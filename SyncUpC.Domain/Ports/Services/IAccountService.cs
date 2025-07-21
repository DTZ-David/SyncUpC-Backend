using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Ports.Services;


public interface IAccountService
{
    public Task<string> ValidateMobileApp(string email, string password);

    public Task<RefreshToken> RefreshToken(string userId);
}
