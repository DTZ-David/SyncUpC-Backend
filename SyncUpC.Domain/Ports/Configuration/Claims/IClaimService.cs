using SyncUpC.Domain.Settings.Claims;

namespace SyncUpC.Domain.Ports.Configuration.Claims;

public interface IClaimService
{
    Task<UserClaim> GetUserClaim();
}
