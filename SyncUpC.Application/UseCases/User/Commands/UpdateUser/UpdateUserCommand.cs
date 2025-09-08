using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.User.Commands.UpdateUser;

public record UpdateUserCommand(string? ProfilePicture, string? Name, string? Email, string? PhoneNumber) : IRequest<ActionResult<Response<UserDto>>>;
