using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Events.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, ActionResult<Response<AcademicEventDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult<Response<AcademicEventDto>>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            // 🔒 validar usuario autenticado
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

            // 🔎 obtener el evento
            var academicEvent = await _unitOfWork.EventService.GetEventById(request.id);
            if (academicEvent == null)
                throw new BusinessException("El evento no existe", (int)MessageStatusCode.NotFound);

            // ❌ eliminar
            await _unitOfWork.EventService.DeleteEvent(request.id);

            // 📦 mapear dto del evento eliminado
            var dto = _mapper.Map<AcademicEventDto>(academicEvent);

            return new CreatedResult(string.Empty, new Response<AcademicEventDto>((int)MessageStatusCode.Success, dto));
        }
    }
}
