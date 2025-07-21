namespace SyncUpC.Application.UseCases.User.Dtos;


public record StudentDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string ProfilePhotoUrl,
    CareerDto Career
);

public record CareerDto(
    string Name
);
