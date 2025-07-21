namespace SyncUpC.Domain.Ports.Configuration.JsonWebToken;

public interface IJwtService
{
    string BuildToken(List<string> claimsValue);
    string GenerateRefreshToken();
    Task<bool> ValidateTokenAsync(string token);
    DateTime GetTokenExpiration();
}
