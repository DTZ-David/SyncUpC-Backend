using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, ActionResult<Response<AcademicEventDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEventCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult<Response<AcademicEventDto>>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

            var careers = new List<Domain.Entities.User.Career>();
            foreach (var id in request.CareerIds)
            {
                var career = await _unitOfWork.CareerService.GetCareerById(id);
                if (career != null) careers.Add(career);
            }

            // Buscar el evento
            var academicEvent = await _unitOfWork.EventService.GetEventById(request.EventId);
            if (academicEvent == null)
                throw new BusinessException("El evento no existe", (int)MessageStatusCode.NotFound);

            // Actualizar propiedades
            academicEvent.EventTitle = request.EventTitle;
            academicEvent.EventObjective = request.EventObjective;
            academicEvent.EventLocation = request.EventLocation;
            academicEvent.Address = request.Address;
            academicEvent.StartDate = request.StartDate;
            academicEvent.EndDate = request.EndDate;
            academicEvent.RegistrationStart = request.RegistrationStart;
            academicEvent.RegistrationEnd = request.RegistrationEnd;
            academicEvent.Careers = careers;
            academicEvent.TargetTeachers = request.TargetTeachers;
            academicEvent.TargetStudents = request.TargetStudents;
            academicEvent.TargetAdministrative = request.TargetAdministrative;
            academicEvent.TargetGeneral = request.TargetGeneral;
            academicEvent.IsVirtual = request.IsVirtual;
            academicEvent.MeetingUrl = request.MeetingUrl;
            academicEvent.MaxCapacity = request.MaxCapacity;
            academicEvent.RequiresRegistration = request.RequiresRegistration;
            academicEvent.IsPublic = request.IsPublic;
            academicEvent.Tags = request.Tags;
            academicEvent.ImageUrls = request.ImageUrls ?? new List<string>();
            academicEvent.AdditionalDetails = request.AdditionalDetails!;

            // Guardar cambios
            await _unitOfWork.EventService.UpdateEvent(academicEvent);


            var resultDto = _mapper.Map<AcademicEventDto>(academicEvent);

            return new CreatedResult(string.Empty, new Response<AcademicEventDto>((int)MessageStatusCode.Create, resultDto));
        }
    }
}
