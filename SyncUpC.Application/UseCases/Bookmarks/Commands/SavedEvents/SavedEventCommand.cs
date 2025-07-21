using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Bookmarks.Commands.SavedEvents;

public record SavedEventCommand(string eventId) : IRequest<ActionResult<Response<AcademicEventDto>>>;
