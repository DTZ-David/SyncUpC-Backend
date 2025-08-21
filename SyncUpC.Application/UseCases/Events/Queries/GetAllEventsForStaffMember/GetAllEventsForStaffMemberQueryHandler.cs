using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Events.Queries.GetAllEventsForStaffMember;

public class GetAllEventsForStaffMemberQueryHandler : IRequestHandler<GetAllEventsForStaffMemberQuery, ActionResult<Response<IEnumerable<AcademicEventDto>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllEventsForStaffMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<AcademicEventDto>>>> Handle(GetAllEventsForStaffMemberQuery request, CancellationToken cancellationToken)
    {
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        if (user is not StaffMember staffMember)
        {
            throw new BusinessException("Error:", (int)MessageStatusCode.BadRequest);
        }

        var events = await _unitOfWork.EventService.GetEventsMadeForU(user.Id);

        var orderedEvents = events.OrderBy(e => e.StartDate);
        // Obtener los IDs de eventos favoritos desde el usuario
        var favoriteEventIds = user.FavoriteEventIds ?? new List<string>();

        // Construir manualmente los DTOs incluyendo el campo IsSaved
        var resultDto = orderedEvents.Select(e => new AcademicEventDto(
            e.Id,
            e.EventTitle,
            e.EventObjective,
            e.StartDate,
            e.EventLocation,
            e.TargetTeachers,
            e.TargetStudents,
            e.TargetAdministrative,
            e.TargetGeneral,
            e.AdditionalDetails,
            e.ImageUrls,
            e.ParticipantProfilePictures,
            e.Tags,
            favoriteEventIds.Contains(e.Id.ToString())
        ));

        return new OkObjectResult(new Response<IEnumerable<AcademicEventDto>>((int)MessageStatusCode.Success, resultDto));
    }
}
