using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Ports.Configuration.JsonWebToken;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SyncUpC.Infraestructure.Adapters.Configuration.JsonWebToken;

public class JwtService : IJwtService
{
    #region Constructor
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration) 
    {
        _configuration = configuration;
    }
    #endregion

    #region Public Methods
    public string BuildToken(List<string> claimsValue)
    {
        var secretKey = Encoding.UTF8.GetBytes(
            _configuration["JwtSettings:SecretKey"]
            ?? throw new Exception("JwtSettings:SecretKey no está configurado")
        );

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var encryptionKey = Convert.FromBase64String(
          _configuration["JwtSettings:EncryptKey"]
          ?? throw new Exception("JwtSettings:EncryptKey no está configurado")
        );



        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var expirationMinutes = int.Parse(
            _configuration["JwtSettings:ExpirationMinutes"]
            ?? throw new Exception("JwtSettings:ExpirationMinutes no está configurado")
        );

        var responseClaims = BuildClaims(claimsValue);

        var tokenDescription = new SecurityTokenDescriptor
        {
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(responseClaims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(securityToken);
    }



    #endregion

    #region Private Methods
    private List<Claim> BuildClaims(List<string> claimsValue)
    {
        List<string> customClaimTypes = new()
{
    ClaimOption.UserId,
    ClaimOption.Email,

};

        var responseClaims = new List<Claim>();

        for (int i = 0; i < customClaimTypes.Count; i++)
        {
            responseClaims.Add(new Claim(customClaimTypes[i], claimsValue[i]));
        }

        return responseClaims;
    }

    #endregion
}
