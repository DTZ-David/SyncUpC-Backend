using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.User.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken)
  : IRequest<ActionResult<Response<TokenDto>>>;
