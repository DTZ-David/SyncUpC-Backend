using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Application.UseCases.User.Students.Dtos;


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
    string Name,
    FacultyDto Faculty
);

public record FacultyDto(
    string Name
);
