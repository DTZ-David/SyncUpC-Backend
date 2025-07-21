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
    private readonly IRefreshTokenService _refreshTokenService;

    public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService, IRefreshTokenService refreshTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<ActionResult<Response<TokenDto>>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existingToken = await _refreshTokenService.GetByTokenAsync(request.RefreshToken);

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

        // ✅ Actualiza el documento de refresh token con uno nuevo
        var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

        var tokenDto = new TokenDto(newAccessToken, newRefreshToken.Token);
        return new OkObjectResult(new Response<TokenDto>((int)MessageStatusCode.Success, tokenDto));
    }

}
