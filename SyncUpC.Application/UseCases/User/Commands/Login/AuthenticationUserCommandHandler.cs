using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.User.Commands.Login;

public class AuthenticationUserCommandHandler : IRequestHandler<AuthenticationUserCommand, ActionResult<Response<AuthenticationUserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<AuthenticationUserDto>>> Handle(AuthenticationUserCommand request, CancellationToken cancellationToken)
    {
        // Acceso a token generado desde AccountService
        var accessToken = await _unitOfWork.AccountService.ValidateMobileApp(request.email, request.password);

        var user = await _unitOfWork.UserService.GetUserByEmail(request.email);

        if (user is null)
        {
            throw new BusinessException("Usuario no encontrado.", (int)MessageStatusCode.NotFound);
        }

        // ✅ Generar el refresh token aquí
        var refreshToken = await _unitOfWork.RefreshTokenService.GenerateRefreshTokenAsync(user.Id);

        var userDetails = new AuthenticationUserDto(
            Token: accessToken,
            RefreshToken: refreshToken.Token,
            Name: $"{user.Name} {user.LastName}",
            ProfilePicture: user.ProfilePicture ?? "",
            Role: user.Role.ToString()
        );

        return new Response<AuthenticationUserDto>((int)MessageStatusCode.Success, userDetails);
    }

}
