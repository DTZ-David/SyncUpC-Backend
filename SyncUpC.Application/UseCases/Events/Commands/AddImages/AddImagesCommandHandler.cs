using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Events.Commands.AddImages;

internal class AddImagesCommandHandler : IRequestHandler<AddImagesCommand, ActionResult<Response<EventImagesDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddImagesCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<EventImagesDto>>> Handle(AddImagesCommand request, CancellationToken cancellationToken)
    {
        // Obtener usuario autenticado
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        // Verificar que el evento existe
        var academicEvent = await _unitOfWork.EventService.GetEventById(request.EventId);
        if (academicEvent == null)
            throw new BusinessException("El evento no existe", (int)MessageStatusCode.NotFound);

        // Validar que se proporcionaron URLs de imágenes
        if (request.ImageUrls == null || !request.ImageUrls.Any())
            throw new BusinessException("Debe proporcionar al menos una URL de imagen", (int)MessageStatusCode.BadRequest);

        // Crear el registro de auditoría de imágenes
        var eventImages = new EventImages(
            eventId: request.EventId,
            imageUrls: request.ImageUrls,
            uploadedByUserId: user.Id,
            uploadedByUserName: $"{user.Name} {user.LastName}",
            eventDate: academicEvent.StartDate,
            eventTitle: academicEvent.EventTitle
        );

        // Guardar en la colección EventImages
        await _unitOfWork.EventImageService.CreateEventImages(eventImages);

        // Opcional: Actualizar también la lista de imágenes en el evento principal
        // Esto mantiene compatibilidad con el código existente
        var currentImages = academicEvent.ImageUrls ?? new List<string>();
        var newImages = currentImages.Union(request.ImageUrls).ToList();
        academicEvent.ImageUrls = newImages;
        await _unitOfWork.EventService.UpdateEvent(academicEvent);

        // Crear DTO de respuesta
        var resultDto = new EventImagesDto(
            eventImages.Id,
            eventImages.EventId,
            eventImages.EventTitle,
            eventImages.EventDate,
            eventImages.ImageUrls,
            eventImages.UploadedByUserId,
            eventImages.UploadedByUserName
        );

        return new OkObjectResult(new Response<EventImagesDto>((int)MessageStatusCode.Success, resultDto));
    }
}