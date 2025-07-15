using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Application.UseCases.Events.Commands.CreateEvent;

public record CreateEventCommand(
    // Info básica
    string EventTitle,
    string EventObjective,
    string EventLocation,
    string Address,

    DateTime StartDate,
    DateTime EndDate,
    DateTime RegistrationStart,
    DateTime RegistrationEnd,

    List<EventCategory> CategoryId,
    string FacultyId,
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
    List<string> Tags,
    List<string>? ImageUrls,
    string? AdditionalDetails
) : IRequest<ActionResult<Response<AcademicEventDto>>>;
