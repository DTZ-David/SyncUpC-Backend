using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Emails.Commands.SendEmailsForEvent;

public record SendEmailsForEventCommand(string eventId) : IRequest<ActionResult<Response<AcademicEventDto>>>;

