using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Configuration.JsonWebToken;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;


[ApplicationService]
public class AccountService : IAccountService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IJwtService _jwtService;

    public AccountService(IGenericRepository<User> userRepository,
                          IJwtService jwtService
                        )
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }




    public async Task<string> ValidateMobileApp(string email, string password)
    {
        List<string> claimsValue = [];
        var findUser = await ValidateCredentials(email, password);

        // Public Claims 
        claimsValue.Add(findUser.Id);
        claimsValue.Add(findUser.Email);
        claimsValue.Add(findUser.Role.ToString());
        claimsValue.Add("4076de2f-91cc-4e5c-9bb9-e252489ef313");

        var token = _jwtService.BuildToken(claimsValue);

        return token;
    }

    private async Task<User> ValidateCredentials(string email, string password)
    {
        var user = (await _userRepository.FindAsync(
                    u => u.Email == email && u.Password == password)).FirstOrDefault();

        if (user == null)
        {
            throw new BusinessException("El correo o la contraseña son incorrectos.", (int)MessageStatusCode.Unauthorized);
        }

        return user;
    }

}