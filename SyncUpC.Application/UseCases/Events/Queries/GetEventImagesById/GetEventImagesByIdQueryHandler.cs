using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Events.Queries.GetEventImagesById;

internal class GetEventImagesByIdQueryHandler : IRequestHandler<GetEventImagesByIdQuery, ActionResult<Response<List<EventImagesDto>>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetEventImagesByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<List<EventImagesDto>>>> Handle(GetEventImagesByIdQuery request, CancellationToken cancellationToken)
    {
        // Verificar autenticación
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        // Verificar que el evento existe
        var academicEvent = await _unitOfWork.EventService.GetEventById(request.EventId);
        if (academicEvent == null)
            throw new BusinessException("El evento no existe", (int)MessageStatusCode.NotFound);

        // Obtener todas las imágenes del evento
        var eventImages = await _unitOfWork.EventImageService.GetEventImagesByUserId(request.EventId);

        // Mapear a DTOs
        var eventImagesDtos = eventImages.Select(ei => new EventImagesDto(
            ei.Id,
            ei.EventId,
            ei.EventTitle,
            ei.EventDate,
            ei.ImageUrls,
            ei.UploadedByUserId,
            ei.UploadedByUserName
        )).ToList();

        return new OkObjectResult(new Response<List<EventImagesDto>>((int)MessageStatusCode.Success, eventImagesDtos));
    }
}