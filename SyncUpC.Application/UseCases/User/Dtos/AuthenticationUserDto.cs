namespace SyncUpC.Application.UseCases.User.Dtos;

public record AuthenticationUserDto(
    string Token,
    string RefreshToken,  // ✅ NUEVO
    string Name,
    string ProfilePicture,
    string Role
);
public record AccountDto(string Email, string Password);
