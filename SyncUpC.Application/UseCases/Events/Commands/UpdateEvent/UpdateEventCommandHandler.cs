using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;
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

            // Buscar evento existente
            var academicEvent = await _unitOfWork.EventService.GetEventById(request.EventId);
            if (academicEvent == null)
                throw new BusinessException("El evento no existe", (int)MessageStatusCode.NotFound);

            // Carreras
            var careers = new List<Domain.Entities.User.Career>();
            foreach (var id in request.CareerIds)
            {
                var career = await _unitOfWork.CareerService.GetCareerById(id);
                if (career != null) careers.Add(career);
            }

            // Campus y espacio
            var campus = await _unitOfWork.CampusService.GetCampusById(request.CampusId)
                ?? throw new BusinessException("Campus no encontrado", (int)MessageStatusCode.NotFound);

            var space = await _unitOfWork.SpaceService.GetSpaceById(request.SpaceId)
                ?? throw new BusinessException("Espacio no encontrado", (int)MessageStatusCode.NotFound);

            // Categorías
            var categories = new List<EventCategory>();
            foreach (var id in request.EventCategoryId)
            {
                var category = await _unitOfWork.EventCategoryService.GetCategoryById(id);
                if (category != null) categories.Add(category);
            }

            // Tipos de evento
            var eventTypes = new List<EventType>();
            foreach (var id in request.EventTypesId)
            {
                var type = await _unitOfWork.EventTypeService.GetEventType(id);
                if (type != null) eventTypes.Add(type);
            }

            // Actualizar propiedades
            academicEvent.EventTitle = request.EventTitle;
            academicEvent.EventObjective = request.EventObjective;
            academicEvent.StartDate = request.StartDate;
            academicEvent.EndDate = request.EndDate;
            academicEvent.Campus = campus;
            academicEvent.Space = space;
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
            academicEvent.Categories = categories;
            academicEvent.EventTypes = eventTypes;
            academicEvent.ImageUrls = request.ImageUrls ?? new List<string>();
            academicEvent.AdditionalDetails = request.AdditionalDetails ?? string.Empty;

            // Guardar cambios
            await _unitOfWork.EventService.UpdateEvent(academicEvent);

            var resultDto = new AcademicEventDto(
                academicEvent.Id,
                academicEvent.EventTitle,
                academicEvent.EventObjective,
                academicEvent.StartDate,
                academicEvent.EndDate,
                new CampusDto(campus.Name),
                new SpaceDto(space.Name),
                academicEvent.TargetTeachers,
                academicEvent.TargetStudents,
                academicEvent.TargetAdministrative,
                academicEvent.TargetGeneral,
                academicEvent.AdditionalDetails,
                academicEvent.ImageUrls ?? new List<string>(),
                academicEvent.ParticipantProfilePictures ?? new List<string>(),
                categories.Select(c => new EventCategoryDto(c.Name)).ToList(),
                eventTypes.Select(et => new EventTypeDto(et.Name)).ToList(),
                academicEvent.RequiresRegistration,
                false,
                false,
                academicEvent.Status
            );

            return new OkObjectResult(new Response<AcademicEventDto>((int)MessageStatusCode.Success, resultDto));
        }
    }
}
