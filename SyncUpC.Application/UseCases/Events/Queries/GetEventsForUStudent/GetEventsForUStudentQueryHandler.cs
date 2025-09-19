using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Events.Queries.GetEventsForU
{
    public class GetEventsForUStudentQueryHandler : IRequestHandler<GetEventsForUStudentQuery, ActionResult<Response<IEnumerable<AcademicEventDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventsForUStudentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActionResult<Response<IEnumerable<AcademicEventDto>>>> Handle(GetEventsForUStudentQuery request, CancellationToken cancellationToken)
        {
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

            IEnumerable<AcademicEvent> events;

            if (user is Student student)
            {
                // 🔹 Si es estudiante → eventos filtrados por carrera
                events = await _unitOfWork.EventService.GetEventsForU(student.Career.Id);
            }
            else if (user is StaffMember)
            {
                // 🔹 Si es staff → todos los eventos (sin filtro)
                events = await _unitOfWork.EventService.GetAllEvents();
            }
            else
            {
                throw new BusinessException("El usuario no tiene permisos para consultar eventos", (int)MessageStatusCode.BadRequest);
            }
            var userRegistrations = await _unitOfWork.RegistrationService.GetAllRegistration();
            var userRegisteredEventIds = userRegistrations
                .Where(r => r.RegistratedUsers.Any(ru => ru.UserId == userClaim.UserId))
                .Select(r => r.EventId)
                .ToHashSet();

            var orderedEvents = events.OrderBy(e => e.StartDate);

            // Obtener los IDs de eventos favoritos desde el usuario
            var favoriteEventIds = user.FavoriteEventIds ?? new List<string>();

            // Construir manualmente los DTOs incluyendo el campo IsSaved
            var resultDto = orderedEvents.Select(e => new AcademicEventDto(
                e.Id,
                e.EventTitle,
                e.EventObjective,
                e.StartDate,
                e.EndDate,

                // Ubicación
                new CampusDto(e.Campus.Name),
                new SpaceDto(e.Space.Name),

                // Públicos objetivos
                e.TargetTeachers,
                e.TargetStudents,
                e.TargetAdministrative,
                e.TargetGeneral,

                // Extras
                e.AdditionalDetails,
                e.ImageUrls,
                e.ParticipantProfilePictures,

                // Clasificación
                e.Categories?.Select(c => new EventCategoryDto(c.Name)).ToList() ?? new List<EventCategoryDto>(),
                e.EventTypes?.Select(t => new EventTypeDto(t.Name)).ToList() ?? new List<EventTypeDto>(),
                e.RequiresRegistration,
                // Favorito
                favoriteEventIds.Contains(e.Id.ToString()),
                userRegisteredEventIds.Contains(e.Id.ToString()),
                e.Status
            ));

            return new OkObjectResult(new Response<IEnumerable<AcademicEventDto>>((int)MessageStatusCode.Success, resultDto));
        }



    }
}
