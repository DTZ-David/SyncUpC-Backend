using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Events.Commands.DeleteEventImages;

public class DeleteEventImagesCommandHandler : IRequestHandler<DeleteEventImagesCommand, ActionResult<Response<bool>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventImagesCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<bool>>> Handle(DeleteEventImagesCommand request, CancellationToken cancellationToken)
    {
        // Verificar autenticación
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        // Verificar que las imágenes existen
        var eventImages = await _unitOfWork.EventImageService.GetEventImagesById(request.Id);
        if (eventImages == null)
            throw new BusinessException("Las imágenes del evento no existen", (int)MessageStatusCode.NotFound);

        // Eliminar las imágenes
        await _unitOfWork.EventImageService.DeleteEventImages(request.Id);

        return new OkObjectResult(new Response<bool>((int)MessageStatusCode.Success, true));
    }
}