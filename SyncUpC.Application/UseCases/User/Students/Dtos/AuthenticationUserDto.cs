using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Application.UseCases.User.Students.Dtos;

public record AuthenticationUserDto(
 string Token,
 string Name,
 string ProfilePicture
);

public record AccountDto(string Email, string Password);
