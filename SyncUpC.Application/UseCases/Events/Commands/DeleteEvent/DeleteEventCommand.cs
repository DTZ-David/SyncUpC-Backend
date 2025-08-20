using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Events.Commands.DeleteEvent
{
    public record DeleteEventCommand(string id) : IRequest<ActionResult<Response<AcademicEventDto>>>;
}
