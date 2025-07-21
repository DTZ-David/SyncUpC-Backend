using SyncUpC.Domain.Entities.Base;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Configuration.JsonWebToken;
using SyncUpC.Domain.Services;
using System.Security.Cryptography;


[ApplicationService]
public class RefreshTokenService : IRefreshTokenService
{
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;

    public RefreshTokenService(IGenericRepository<RefreshToken> refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokenRepository.Add(refreshToken);
        return refreshToken;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        var results = await _refreshTokenRepository.FindAsync(rt => rt.Token == token);
        return results.FirstOrDefault();
    }

    public async Task RevokeAsync(RefreshToken token, string? replacedBy = null)
    {
        token.RevokedAt = DateTime.UtcNow;
        token.ReplacedByToken = replacedBy;
        await _refreshTokenRepository.Update(token);
    }
}
