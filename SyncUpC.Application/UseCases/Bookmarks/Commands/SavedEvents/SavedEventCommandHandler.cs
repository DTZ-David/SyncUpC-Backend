using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Bookmarks.Commands.SavedEvents
{
    public class SavedEventCommandHandler : IRequestHandler<SavedEventCommand, ActionResult<Response<AcademicEventDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SavedEventCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult<Response<AcademicEventDto>>> Handle(SavedEventCommand request, CancellationToken cancellationToken)
        {
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();

            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("Usuario no encontrado", (int)MessageStatusCode.NotFound);

            var academicEvent = await _unitOfWork.EventService.GetEventById(request.eventId)
                ?? throw new BusinessException("Evento no encontrado", (int)MessageStatusCode.NotFound);

            // Verificar si el evento ya está guardado
            if (user.FavoriteEventIds.Contains(request.eventId))
            {
                throw new BusinessException("El evento ya está guardado", (int)MessageStatusCode.Conflict);
            }

            // Agregar evento a favoritos
            user.FavoriteEventIds.Add(request.eventId);

            await _unitOfWork.UserService.UpdateUser(user);

            var eventDto = _mapper.Map<AcademicEventDto>(academicEvent);

            return new CreatedResult(string.Empty, new Response<AcademicEventDto>((int)MessageStatusCode.Create, eventDto));
        }
    }
}
