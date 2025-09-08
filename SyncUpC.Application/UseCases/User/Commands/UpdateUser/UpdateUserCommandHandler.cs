using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.User.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ActionResult<Response<UserDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<UserDto>>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
        var userToUpdate = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);



        // Actualizar solo los campos que han sido proporcionados (no nulos/vacíos)
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            userToUpdate.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            userToUpdate.PhoneNumber = request.PhoneNumber;
        }

        if (!string.IsNullOrWhiteSpace(request.ProfilePicture))
        {
            userToUpdate.ProfilePicture = request.ProfilePicture;
        }

        // El email normalmente no se actualiza por seguridad, pero si es requerido:
        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != userToUpdate.Email)
        {
            // Verificar que el nuevo email no esté en uso
            var existingUser = await _unitOfWork.UserService.GetUserByEmail(request.Email);
            if (existingUser != null && existingUser.Id != userToUpdate.Id)
            {
                throw new BusinessException("El email ya está en uso", (int)MessageStatusCode.Conflict);
            }
            userToUpdate.Email = request.Email;
        }

        // Guardar cambios
        await _unitOfWork.UserService.UpdateUser(userToUpdate);

        // Crear el DTO de respuesta
        var resultDto = new UserDto(
            userToUpdate.Name,
            userToUpdate.PhoneNumber,
            userToUpdate.ProfilePicture,
            userToUpdate.Email
        );

        return new OkObjectResult(new Response<UserDto>((int)MessageStatusCode.Success, resultDto));
    }
}
