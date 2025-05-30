using SyncUpC.Domain.Ports.Configuration.Claims;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Ports;

public interface IUnitOfWork
{
    IAccountService AccountService { get; }
    IClaimService ClaimsService { get; }
}
