using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Students.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.User.Students.Commands.Login;

public class AuthenticationUserCommandHandler : IRequestHandler<AuthenticationUserCommand, ActionResult<Response<AuthenticationUserDto>>>
{ 
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<AuthenticationUserDto>>> Handle(AuthenticationUserCommand request, CancellationToken cancellationToken)
    {
        var token = await _unitOfWork.AccountService.ValidateMobileApp(request.email, request.password);
        var user = await _unitOfWork.StudentService.GetUserByEmail(request.email);
        if (user is null)
        {
            throw new BusinessException($"Usuario no encontrado.",
                (int)MessageStatusCode.NotFound);
        }

        var userDetails = new AuthenticationUserDto(
                Token: token,
                Name: user.Name + " " + user.LastName,
                ProfilePicture: user.ProfilePicture!
                );


        return new Response<AuthenticationUserDto>((int)MessageStatusCode.Succes, userDetails);

    }
}
