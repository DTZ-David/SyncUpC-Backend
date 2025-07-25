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

            var careers = new List<Domain.Entities.User.Career>();

            foreach (var id in request.CareerIds)
            {
                var career = await _unitOfWork.CareerService.GetCareerById(id);
                careers.Add(career);
            }

            var eventStats = new EventStats(0, 0, 0);

            var newEvent = new AcademicEvent(
                organizer,
                request.EventTitle,
                request.EventObjective,
                request.StartDate,
                request.EndDate,
                request.RegistrationStart,
                request.RegistrationEnd,
                careers,
                request.EventLocation,
                request.TargetTeachers,
                request.TargetStudents,
                request.TargetAdministrative,
                request.TargetGeneral,
                request.Address,
                request.IsVirtual,
                request.MeetingUrl,
                request.MaxCapacity,
                request.RequiresRegistration,
                request.IsPublic,
                "draft", // o inicializado con un enum si lo deseas
                request.Tags,
                eventStats,
                request.AdditionalDetails!,
                request.ImageUrls!,
                new List<string>() // sin participantes al crear
            );

            await _unitOfWork.EventService.CreateEventAsync(newEvent);

            var resultDto = _mapper.Map<AcademicEventDto>(newEvent);

            return new CreatedResult(string.Empty, new Response<AcademicEventDto>((int)MessageStatusCode.Create, resultDto));
        }
    }

}
