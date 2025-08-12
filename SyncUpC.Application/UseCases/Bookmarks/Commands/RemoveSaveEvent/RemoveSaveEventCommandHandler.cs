using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;


namespace SyncUpC.Application.UseCases.Bookmarks.Commands.RemoveSaveEvent;

public class RemoveSaveEventCommandHandler : IRequestHandler<RemoveSaveEventCommand, ActionResult<Response<AcademicEventDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveSaveEventCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<AcademicEventDto>>> Handle(RemoveSaveEventCommand request, CancellationToken cancellationToken)
    {
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();

        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("Usuario no encontrado", (int)MessageStatusCode.NotFound);

        var academicEvent = await _unitOfWork.EventService.GetEventById(request.eventId)
            ?? throw new BusinessException("Evento no encontrado", (int)MessageStatusCode.NotFound);

        // Verificar que el evento esté en la lista de favoritos
        if (!user.FavoriteEventIds.Contains(request.eventId))
        {
            throw new BusinessException("El evento no está guardado", (int)MessageStatusCode.Conflict);
        }

        // Remover evento de favoritos
        user.FavoriteEventIds.Remove(request.eventId);

        await _unitOfWork.UserService.UpdateUser(user);

        var eventDto = _mapper.Map<AcademicEventDto>(academicEvent);

        return new OkObjectResult(new Response<AcademicEventDto>((int)MessageStatusCode.Success, eventDto));
    }

}
