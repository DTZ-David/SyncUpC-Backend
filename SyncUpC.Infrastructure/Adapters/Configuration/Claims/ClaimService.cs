using Microsoft.AspNetCore.Http;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Ports.Configuration.Claims;
using SyncUpC.Domain.Settings.Claims;
using System.Security.Claims;

namespace SyncUpC.Infraestructure.Adapters.Configuration.Claims;

public class ClaimService : IClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<UserClaim> GetUserClaim()
    {
        var user = new UserClaim(GetClaim(ClaimOption.UserId),
            GetClaim(ClaimOption.Email),
            GetClaim(ClaimOption.Role));
        return Task.FromResult(user);
    }

    public string GetClaim(string name)
    {
        if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var currentUser = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity!;
            var claim = currentUser.Claims.FirstOrDefault(c => c.Type.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (claim is not null)
                return claim.Value;
        }

        return string.Empty;
    }


}
