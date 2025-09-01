using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Events.Commands.UpdateEvent;

public record UpdateEventCommand(
    string EventId,

    // Info básica
    string EventTitle,
    string EventObjective,
    string CampusId,
    string SpaceId,

    DateTime StartDate,
    DateTime EndDate,
    List<string> CareerIds,

    bool TargetTeachers,
    bool TargetStudents,
    bool TargetAdministrative,
    bool TargetGeneral,

    bool IsVirtual,
    string? MeetingUrl,
    int MaxCapacity,
    bool RequiresRegistration,
    bool IsPublic,

    List<string> EventTypesId,
    List<string> EventCategoryId,
    List<string>? ImageUrls,
    string? AdditionalDetails
) : IRequest<ActionResult<Response<AcademicEventDto>>>;

