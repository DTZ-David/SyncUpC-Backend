namespace SyncUpC.Application.UseCases.User.Dtos;

public record StaffMemberDto(
     string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string ProfilePhotoUrl,
    string Profession,
    string Department,
    string Position,
    FacultyDto Faculty
    );

public record FacultyDto(
    string Name
    );