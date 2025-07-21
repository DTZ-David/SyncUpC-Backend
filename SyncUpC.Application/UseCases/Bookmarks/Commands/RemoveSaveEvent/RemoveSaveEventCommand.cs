using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Bookmarks.Commands.RemoveSaveEvent
{
    public record RemoveSaveEventCommand(string eventId) : IRequest<ActionResult<Response<AcademicEventDto>>>;

}
