using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;


namespace SyncUpC.Application.UseCases.User.Commands.Login;

public record AuthenticationUserCommand(string email, string password) : IRequest<ActionResult<Response<AuthenticationUserDto>>>;
