using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Application.UseCases.Events.Queries.GetUserEventImages;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Events.Queries.GetEventImages;

public class GetUserEventImagesQueryHandler : IRequestHandler<GetUserEventImagesQuery, ActionResult<Response<List<EventImagesDto>>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetUserEventImagesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<List<EventImagesDto>>>> Handle(GetUserEventImagesQuery request, CancellationToken cancellationToken)
    {
        // Verificar autenticación
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        // Obtener todas las imágenes subidas por el usuario
        var eventImages = await _unitOfWork.EventImageService.GetEventImagesByUserId(user.Id);

        // Mapear a DTOs
        var eventImagesDtos = eventImages.Select(ei => new EventImagesDto(
            ei.Id,
            ei.EventId,
            ei.ImageUrls,
            ei.UploadedByUserId,
            ei.UploadedByUserName,
            ei.UploadedAt,
            ei.Description
        )).ToList();

        return new OkObjectResult(new Response<List<EventImagesDto>>((int)MessageStatusCode.Success, eventImagesDtos));
    }
}