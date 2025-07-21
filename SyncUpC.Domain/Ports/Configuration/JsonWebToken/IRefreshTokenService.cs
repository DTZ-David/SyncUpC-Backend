using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Ports.Configuration.JsonWebToken;

public interface IRefreshTokenService
{
    Task<RefreshToken> GenerateRefreshTokenAsync(string userId);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task RevokeAsync(RefreshToken token, string? replacedBy = null);
}
