using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Configuration.JsonWebToken;

namespace SyncUpC.Application.UseCases.User.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ActionResult<Response<TokenDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<ActionResult<Response<TokenDto>>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existingToken = await _unitOfWork.RefreshTokenService.GetByTokenAsync(request.RefreshToken);

        if (existingToken is null || !existingToken.IsActive)
        {
            return new OkObjectResult(new Response<string>((int)MessageStatusCode.Unauthorized, "Refresh token inválido o expirado."));
        }

        var user = await _unitOfWork.UserService.GetUserById(existingToken.UserId);
        if (user is null)
        {
            return new NotFoundObjectResult(new Response<string>((int)MessageStatusCode.NotFound, "Usuario no encontrado."));
        }

        var claims = new List<string> { user.Id, user.Email };
        var newAccessToken = _jwtService.BuildToken(claims);
        var newRefreshToken = await _unitOfWork.RefreshTokenService.GenerateRefreshTokenAsync(user.Id);

        await _unitOfWork.RefreshTokenService.RevokeAsync(existingToken, newRefreshToken.Token);

        var tokenDto = new TokenDto(newAccessToken, newRefreshToken.Token);
        return new OkObjectResult(new Response<TokenDto>((int)MessageStatusCode.Success, tokenDto));
    }
}
