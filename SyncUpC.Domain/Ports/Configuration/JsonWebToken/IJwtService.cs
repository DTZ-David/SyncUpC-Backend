namespace SyncUpC.Domain.Ports.Configuration.JsonWebToken;

public interface IJwtService
{
    string BuildToken(List<string> claimsValue);
}
