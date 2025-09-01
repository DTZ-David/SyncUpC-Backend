using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;
namespace SyncUpC.Application.UseCases.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, ActionResult<Response<AcademicEventDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEventCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult<Response<AcademicEventDto>>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

            var organizer = new Organizer(
                user.Id,
                user.Email,
                user.PhoneNumber,
                $"{user.Name} {user.LastName}"
            );

            // Carreras
            var careers = new List<Domain.Entities.User.Career>();
            foreach (var id in request.CareerIds)
            {
                var career = await _unitOfWork.CareerService.GetCareerById(id);
                careers.Add(career);
            }

            var campus = await _unitOfWork.CampusService.GetCampusById(request.CampusId);

            var space = await _unitOfWork.SpaceService.GetSpaceById(request.SpaceId);


            // Categorías
            var categories = new List<EventCategory>();
            foreach (var id in request.EventCategoryId)
            {
                var category = await _unitOfWork.EventCategoryService.GetCategoryById(id);
                categories.Add(category);
            }

            // Tipos de evento
            var eventTypes = new List<EventType>();
            foreach (var id in request.EventTypesId)
            {
                var type = await _unitOfWork.EventTypeService.GetEventType(id);
                eventTypes.Add(type);
            }

            var eventStats = new EventStats(0, 0, 0);

            var newEvent = new AcademicEvent(
                organizer,
                request.EventTitle,
                request.EventObjective,
                request.StartDate,
                request.EndDate,
                campus,
                space,
                careers,
                request.TargetTeachers,
                request.TargetStudents,
                request.TargetAdministrative,
                request.TargetGeneral,
                request.IsVirtual,
                request.MeetingUrl,
                request.MaxCapacity,
                request.RequiresRegistration,
                request.IsPublic,
                "created",
                categories,
                eventTypes,
                request.AdditionalDetails ?? string.Empty,
                request.ImageUrls ?? new List<string>(),
                new List<string>() // sin participantes al crear


            );

            await _unitOfWork.EventService.CreateEventAsync(newEvent);

            if (request.RequiresRegistration)
            {
                var qrImageBytes = _unitOfWork.QRService.GenerateQrImageAsBytes(newEvent.Id);
                var subject = "Código QR de asistencia al evento";
                var body = $"Hola {user.Name},\n\nAdjunto encontrarás el código QR para tu evento \"{newEvent.EventTitle}\".";

                await _unitOfWork.EmailService.SendEmailWithAttachmentAsync(
                    to: user.Email,
                    subject: subject,
                    body: body,
                    attachmentBytes: qrImageBytes,
                    attachmentName: "qr_evento.png"
                );
            }

            var resultDto = new AcademicEventDto(
                newEvent.Id,
                newEvent.EventTitle,
                newEvent.EventObjective,
                newEvent.StartDate,
                newEvent.EndDate,
                new CampusDto(campus.Name),
                new SpaceDto(space.Name),
                newEvent.TargetTeachers,
                newEvent.TargetStudents,
                newEvent.TargetAdministrative,
                newEvent.TargetGeneral,
                newEvent.AdditionalDetails,
                newEvent.ImageUrls ?? new List<string>(),
                newEvent.ParticipantProfilePictures ?? new List<string>(),
                categories.Select(c => new EventCategoryDto(c.Name)).ToList(),
                eventTypes.Select(et => new EventTypeDto(et.Name)).ToList(),
                false,
                newEvent.Status
            );

            return new CreatedResult(string.Empty, new Response<AcademicEventDto>((int)MessageStatusCode.Create, resultDto));
        }

    }

}
